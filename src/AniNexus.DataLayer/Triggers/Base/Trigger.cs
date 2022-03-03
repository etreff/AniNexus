using System.Runtime.CompilerServices;
using AniNexus.Data.Entities;
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
    protected bool IsEntityDeleted(EntityEntry entry, bool detachedIsDeleted = true)
    {
        return entry.State == EntityState.Deleted || (detachedIsDeleted && entry.State == EntityState.Detached);
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
