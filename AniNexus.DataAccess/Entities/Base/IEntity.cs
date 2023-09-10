namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Defines a database entity.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Returns the Id of the entity.
    /// </summary>
    object GetId();
}

/// <summary>
/// Defines a database entity with a primary key.
/// </summary>
/// <typeparam name="TKey">The type of the primary key.</typeparam>
public interface IEntity<TKey> : IEntity
    where TKey : notnull
{
    /// <summary>
    /// The primary key of the entity.
    /// </summary>
    TKey Id { get; }

    /// <inheritdoc />
    object IEntity.GetId()
    {
        return Id;
    }

    internal void SetId(TKey id);
}
