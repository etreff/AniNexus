using AniNexus.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AniNexus.Web.Server.Controllers
{
    public partial class AnimeController : Controller
    {
        private readonly ILogger<AnimeController> Logger;

        public AnimeController(ILogger<AnimeController> logger)
        {
            Logger = logger;
        }

        [HttpGet("/api/anime/{Id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAnimeByIdAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await Task.Yield();

            LogAnimeFetchById(Logger, id);

            return Ok(new AnimeDTO
            {
                Name = $"Test Anime Name {id}"
            });
        }

        [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "Fetching anime information for anime {AnimeId}.")]
        static partial void LogAnimeFetchById(ILogger logger, int animeId);
    }
}
