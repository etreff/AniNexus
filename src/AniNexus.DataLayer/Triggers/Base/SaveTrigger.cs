using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using AniNexus.Collections.Concurrent;
using AniNexus.Data.Entities;
using AniNexus.Reflection;
using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AniNexus.Data.Triggers;

/// <summary>
/// The base class for a save trigger.
/// </summary>
public abstract class SaveTrigger
{
    /// <summary>
    /// The database context.
    /// </summary>
    protected IDbContextFactory<ApplicationDbContext> DbContextFactory { get; }

    private static readonly MethodInfo _deleteAsyncMethod = typeof(SaveTrigger).GetMethods().Single(m => m.Name == nameof(DeleteAsync) && m.IsGenericMethodDefinition && m.GetGenericArguments().Length == 2);
    private static readonly ThreadSafeCache<Type, MethodInfo> _deleteAsyncCache = new(t =>
    {
        var keyType = t.GetInterfaces().Single(t => t.IsTypeOf(typeof(IEntity<>))).GenericTypeArguments[0];
        return _deleteAsyncMethod.MakeGenericMethod(keyType);
    });

    private protected SaveTrigger(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        DbContextFactory = dbContextFactory;
    }

    /// <summary>
    /// Returns whether the entity entry is being deleted or optionally detached.
    /// </summary>
    /// <param name="entry">The entry to check.</param>
    /// <param name="detachedIsDeleted">Whether to treat a detached state as a deletion state.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static bool IsEntityDeleted(EntityEntry entry, bool detachedIsDeleted = true)
    {
        return entry.State == EntityState.Deleted || (detachedIsDeleted && entry.State == EntityState.Detached);
    }

    /// <summary>
    /// Marks elements for removal in the dataset.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="dataset">The dataset to remove from.</param>
    /// <param name="selector">The selector that determines which entities to remove.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    protected static Task DeleteAsync<TEntity>(DbSet<TEntity> dataset, Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity, new()
    {
        // I HATE that we have to do this, but since the entity key is not part of the parameter list the compiler cannot resolve the type.
        // In the case that TEntity does not inherit from Entity<T> and instead inherits from Entity<T, TKey> we would need to explicitly
        // specify the generic types. I'd rather slightly increase memory usage and incur one-time runtime costs to create the method
        // than write unnecessarily verbose code.
        return (Task)_deleteAsyncCache.Get(typeof(TEntity)).Invoke(null, new object[] { dataset, selector, cancellationToken })!;
    }

    private static async Task DeleteAsync<TEntity, TEntityKey>(DbSet<TEntity> dataset, Expression<Func<TEntity, bool>> selector, CancellationToken cancellationToken = default)
        where TEntity : class, IEntity<TEntityKey>, new()
        where TEntityKey : struct, IComparable<TEntityKey>, IEquatable<TEntityKey>
    {
        var entitiesToRemove = await dataset.Where(selector).Select(e => e.Id).ToArrayAsync(cancellationToken);
        foreach (var entityId in entitiesToRemove)
        {
            var entity = new TEntity { Id = entityId };
            dataset.Attach(entity).State = EntityState.Deleted;
        }
    }
}

/// <summary>
/// The base class for triggers that are invoked before the entity is saved.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class BeforeSaveTrigger<TEntity> : SaveTrigger, IBeforeSaveTrigger<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Creates a new <see cref="BeforeSaveTrigger{TEntity}"/> instance.
    /// </summary>
    /// <param name="dbContextFactory">The database context factory.</param>
    protected BeforeSaveTrigger(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    /// <inheritdoc/>
    public abstract Task BeforeSave(ITriggerContext<TEntity> context, CancellationToken cancellationToken);
}

/// <summary>
/// The base class for triggers that are invoked after the entity is saved.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
public abstract class AfterSaveTrigger<TEntity> : SaveTrigger, IAfterSaveTrigger<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Creates a new <see cref="AfterSaveTrigger{TEntity}"/> instance.
    /// </summary>
    /// <param name="dbContextFactory">The database context factory.</param>
    protected AfterSaveTrigger(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        : base(dbContextFactory)
    {
    }

    /// <inheritdoc/>
    public abstract Task AfterSave(ITriggerContext<TEntity> context, CancellationToken cancellationToken);
}
