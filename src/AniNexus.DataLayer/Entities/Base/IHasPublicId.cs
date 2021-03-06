namespace AniNexus.Data.Entities;

/// <summary>
/// When added to the class, the entity will have a public Id of type <typeparamref name="TKey"/> generated for it.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public interface IHasPublicId<TKey>
    where TKey : struct, IEquatable<TKey>, IComparable<TKey>
{
    /// <summary>
    /// The public Id of the entity, used for navigation URLs.
    /// </summary>
    TKey PublicId { get; set; }
}

/// <summary>
/// When added to the class, the entity will have a public Id of type <see cref="int"/> generated for it.
/// </summary>
public interface IHasPublicId : IHasPublicId<int>
{
}
