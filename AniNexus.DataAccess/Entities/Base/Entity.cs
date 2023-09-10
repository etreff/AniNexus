namespace AniNexus.DataAccess.Entities;

/// <summary>
/// The base class that all database entities inherit from.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public abstract class Entity<TEntity, TKey> : IEntity<TKey>
    where TEntity : Entity<TEntity, TKey>
    where TKey : IComparable<TKey>, IEquatable<TKey>
{
    /// <summary>
    /// The primary key of the entity.
    /// </summary>
    public TKey Id => _id;
#pragma warning disable IDE0032 // Use auto property
    private TKey _id = default!;
#pragma warning restore IDE0032 // Use auto property

    void IEntity<TKey>.SetId(TKey id)
    {
        _id = id;
    }
}

/// <summary>
/// The base class that all database entities inherit from.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
public abstract class Entity<TEntity> : Entity<TEntity, Guid>
    where TEntity : Entity<TEntity>
{
}

/// <summary>
/// Metadata methods for an entity.
/// </summary>
public static class Entity
{
    /// <summary>
    /// Returns the default table name for an entity.
    /// </summary>
    public static string GetDefaultTableName<TEntity>()
        => GetDefaultTableName(typeof(TEntity));

    /// <summary>
    /// Returns the default table name for an entity.
    /// </summary>
    public static string GetDefaultTableName(Type type)
    {
        const string entitySuffix = "Entity";

        string tableName = type.Name;
        return tableName.EndsWith(entitySuffix, StringComparison.Ordinal)
            ? tableName.Substring(0, tableName.Length - entitySuffix.Length)
            : tableName;
    }

    /// <summary>
    /// Creates a new entity with the key set to the provided value.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <param name="key">The key.</param>
    public static TEntity Create<TEntity, TKey>(TKey key)
        where TEntity : Entity<TEntity, TKey>, new()
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        return Create<TEntity, TKey>(key, _ => { });
    }

    /// <summary>
    /// Creates a new entity with the key set to the provided value.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="entity">The entity to set the key of.</param>
    /// <exception cref="ArgumentException"><paramref name="entity"/> has a non-default key.</exception>
    public static TEntity Create<TEntity, TKey>(TKey key, TEntity entity)
        where TEntity : Entity<TEntity, TKey>
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        if (entity.Id.CompareTo(default) == 0)
        {
            throw new ArgumentException("Entity cannot have a key already set.");
        }

        ((IEntity<TKey>)entity).SetId(key);

        return entity;
    }

    /// <summary>
    /// Creates a new entity with the key set to the provided value.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="configure">The action to configure the entity.</param>
    public static TEntity Create<TEntity, TKey>(TKey key, Action<TEntity> configure)
        where TEntity : Entity<TEntity, TKey>, new()
        where TKey : IComparable<TKey>, IEquatable<TKey>
    {
        var entity = new TEntity();
        ((IEntity<TKey>)entity).SetId(key);

        configure(entity);

        return entity;
    }
}
