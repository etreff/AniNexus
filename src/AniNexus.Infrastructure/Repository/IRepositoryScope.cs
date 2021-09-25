namespace AniNexus.Repository;

/// <summary>
/// An repository scope that, when disposed, will commit any changes automatically.
/// </summary>
public interface IRepositoryScope : IDisposable
{
    /// <summary>
    /// Gets an anime repository.
    /// </summary>
    IAnimeRepository GetAnimeRepository();

    /// <summary>
    /// Gets a user repository
    /// </summary>
    IUserRepository GetUserRepository();
}

/// <summary>
/// A repository scope that, when dispsoed, will commit any changes asynchronously
/// automatically.
/// </summary>
public interface IAsyncRepositoryScope : IRepositoryScope, IAsyncDisposable
{

}
