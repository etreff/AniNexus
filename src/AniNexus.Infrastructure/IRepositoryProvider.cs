using AniNexus.Models.User;

namespace AniNexus.Infrastructure
{
    /// <summary>
    /// Defines a repository providers and a unit of work.
    /// </summary>
    public interface IRepositoryProvider : IDisposable, IAsyncDisposable
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
}
