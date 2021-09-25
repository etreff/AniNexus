using System.Runtime.CompilerServices;
using AniNexus.Domain;
using AniNexus.Domain.Models;
using AniNexus.Models;
using AniNexus.Models.Anime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AniNexus.Repository.Internal;

internal class DefaultAnimeRepository : DefaultRepositoryBase, IAnimeRepository
{
    public DefaultAnimeRepository(ApplicationDbContext dbContext, ILogger<DefaultAnimeRepository> logger)
        : base(dbContext, logger)
    {

    }

    public async ValueTask<EAnimeAgeRating?> GetAnimeAgeRating(int animeId, int localeId, CancellationToken cancellationToken)
    {
        var query = Context.AnimeReleases.AsNoTracking();
        query = localeId != -1
            ? query.Where(r => r.AnimeId == animeId && r.LocaleId == localeId)
            : query.Where(r => r.AnimeId == animeId && r.IsPrimary);

        var anime = await query.FirstOrDefaultAsync(cancellationToken);

        return anime?.AgeRatingId is not null ? (EAnimeAgeRating)anime.AgeRatingId : null;
    }

    public async ValueTask<EAnimeAgeRating?> GetAnimeAgeRating(string animeName, int localeId, CancellationToken cancellationToken)
    {
        var query = GetPrimaryAnimeReleaseByNameQuery(animeName, localeId);

        // We may not hit the correct anime if shows share a similar name, but for now we will assume the names are unique.
        // In a case like "Name" and "Name (Year)" (Fruits Basket for example), we will just say the caller needs to specify
        // the full name including the year.
        var anime = await query.FirstOrDefaultAsync(cancellationToken);

        return anime?.AgeRatingId is not null ? (EAnimeAgeRating)anime.AgeRatingId : null;
    }

    private static readonly Func<ApplicationDbContext, int, Task<AnimeModel?>> GetAnimeQuery = EF.CompileAsyncQuery((ApplicationDbContext context, int animeId) =>
        context.Anime
            .AsNoTrackingWithIdentityResolution()
            .Include(m => m.Category)
            .Include(m => m.Season)
            .Include(m => m.Releases).ThenInclude(m => m.Names)
            .Include(m => m.Releases).ThenInclude(m => m.Locale)
            .Include(m => m.Releases).ThenInclude(m => m.AgeRating)
            .Include(m => m.Releases).ThenInclude(m => m.Status)
#if SONGMODEL
                .Include(m => m.Releases).ThenInclude(m => m.Episodes).ThenInclude(m => m.OpeningSong)
                .Include(m => m.Releases).ThenInclude(m => m.Episodes).ThenInclude(m => m.EndingSong)
#endif
                .Include(m => m.Releases).ThenInclude(m => m.Trailers)
            .Include(m => m.Releases).ThenInclude(m => m.LiveActors)
            .Include(m => m.Releases).ThenInclude(m => m.VoiceActors)
            .Include(m => m.Reviews).ThenInclude(m => m.User)
            .Include(m => m.Reviews).ThenInclude(m => m.Votes)
            .Include(m => m.ExternalIds)
            .Include(m => m.TwitterHashtags)
            .Include(m => m.Genres).ThenInclude(m => m.Genre)
            .Include(m => m.Tags).ThenInclude(m => m.Tag)
            .Include(m => m.Related)
            // Only include the parent series names in the include results.
            .Include(m => m.Series).ThenInclude(m => m.Series).ThenInclude(m => m.Names)
            // Don't care about company aliases.
            .Include(m => m.Companies).ThenInclude(m => m.Company).ThenInclude(m => m.Name)
            .Include(m => m.Companies).ThenInclude(m => m.Role)
            .Include(m => m.People).ThenInclude(m => m.Person)
            .Include(m => m.Characters).ThenInclude(m => m.Character)
            // Favorites is *not* included for performance reasons. The count is inline in the record.
            //.Include(m => m.Favorites)
            .AsSplitQuery()
            .FirstOrDefault(a => a.Id == animeId)
    );
    public async ValueTask<AnimeDTO?> GetAnimeAsync(int animeId, CancellationToken cancellationToken)
    {
        Logger.LogDebug("Fetching anime by Id: {AnimeId}", animeId);

        var anime = await GetAnimeQuery(Context, animeId);

        return new AnimeDTO();
    }

    public async ValueTask<AnimeDTO?> GetAnimeAsync(string animeName, CancellationToken cancellationToken)
    {
        Logger.LogDebug("Fetching anime by name: {AnimeName}", animeName);

        var release = await GetPrimaryAnimeReleaseByNameQuery(animeName, IAnimeRepository.NativeLocaleId)
            .FirstOrDefaultAsync(cancellationToken);

        return release is not null ? await GetAnimeAsync(release.AnimeId, cancellationToken) : null;
    }

    public async ValueTask<EAnimeCategory?> GetAnimeCategory(int animeId, CancellationToken cancellationToken)
    {
        var anime = await Context.Anime
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == animeId, cancellationToken);

        return anime is not null ? (EAnimeCategory)anime.CategoryId : null;
    }

    public async ValueTask<EAnimeCategory?> GetAnimeCategory(string animeName, CancellationToken cancellationToken)
    {
        var release = await GetPrimaryAnimeReleaseByNameQuery(animeName, IAnimeRepository.NativeLocaleId)
            .Include(r => r.Anime)
            .FirstOrDefaultAsync(cancellationToken);

        return release is not null ? (EAnimeCategory)release.Anime.CategoryId : null;
    }

    public ValueTask<IEnumerable<AnimeDTO>> GetAnimeForSeasonAsync(EAnimeSeason season, int year, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<MediaNameDTO?> GetAnimeNameAsync(int animeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<string?> GetAnimeNameAsync(int animeId, int localeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<AnimeReleaseDTO> GetAnimeReleaseAsync(int animeId, int localeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<AnimeReleaseDTO> GetAnimeReleaseAsync(string animeName, int localeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByDateAsync(DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByDateAsync(DateOnly? startDate, DateOnly? endDate, int localeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByEndDateAsync(DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByEndDateAsync(DateOnly? startDate, DateOnly? endDate, int localeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByStartDateAsync(DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByStartDateAsync(DateOnly? startDate, DateOnly? endDate, int localeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleasesForSeasonAsync(EAnimeSeason season, int year, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseWithRatingAsync(byte minimumRating, byte maximumRating, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<EAnimeSeason?> GetAnimeSeason(int animeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<EAnimeSeason?> GetAnimeSeason(string animeName, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<EMediaStatus> GetAnimeStatus(int animeId, int localeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<EMediaStatus> GetAnimeStatus(string animeName, int localeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<string?> GetAnimeSynopsis(int animeId, int localeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<string?> GetAnimeSynopsis(string animeName, int localeId, CancellationToken cancellationToken) => throw new NotImplementedException();
    public ValueTask<IEnumerable<AnimeDTO>> GetAnimeWithRatingAsync(byte minimumRating, byte maximumRating, CancellationToken cancellationToken) => throw new NotImplementedException();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IQueryable<AnimeReleaseModel> GetPrimaryAnimeReleaseByNameQuery(string animeName, int localeId)
    {
        var query = Context.AnimeReleases.AsNoTracking();
        return localeId != -1
            ? query.Where(r => (r.Name.RomajiName == animeName || r.Name.NativeName == animeName || r.Name.EnglishName == animeName) && r.LocaleId == localeId)
            : query.Where(r => (r.Name.RomajiName == animeName || r.Name.NativeName == animeName || r.Name.EnglishName == animeName) && r.IsPrimary);
    }
}
