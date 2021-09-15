using AniNexus.Models.Anime;

namespace AniNexus.Infrastructure
{
    /// <summary>
    /// A repository for anime.
    /// </summary>
    public interface IAnimeRepository
    {
        /// <summary>
        /// Gets an anime by name.
        /// </summary>
        /// <param name="animeId">The AniNexus Id of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<AnimeDTO> GetAnimeByIdAsync(int animeId, CancellationToken cancellationToken);
    }
}
