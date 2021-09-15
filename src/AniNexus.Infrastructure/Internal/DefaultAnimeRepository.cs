using AniNexus.Domain;
using AniNexus.Models.Anime;
using Microsoft.Extensions.Logging;

namespace AniNexus.Infrastructure.Internal
{
    internal class DefaultAnimeRepository : IAnimeRepository
    {
        private readonly ApplicationDbContext Context;
        private readonly ILogger Logger;

        public DefaultAnimeRepository(ApplicationDbContext dbContext, ILogger<DefaultAnimeRepository> logger)
        {
            Context = dbContext;
            Logger = logger;
        }

        public ValueTask<AnimeDTO> GetAnimeByIdAsync(int animeId, CancellationToken cancellationToken)
        {
            return new ValueTask<AnimeDTO>(new AnimeDTO());
        }
    }
}
