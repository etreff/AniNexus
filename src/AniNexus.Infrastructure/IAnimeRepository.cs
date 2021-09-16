using AniNexus.Models;
using AniNexus.Models.Anime;

namespace AniNexus.Infrastructure
{
    /// <summary>
    /// A repository for anime.
    /// </summary>
    public interface IAnimeRepository
    {
        /// <summary>
        /// Specifies to return a result using the release that contains the
        /// native locale.
        /// </summary>
        public const int NativeLocaleId = -1;

        /// <summary>
        /// Gets an anime by Id.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<AnimeDTO?> GetAnimeAsync(int animeId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets an anime by name.
        /// </summary>
        /// <param name="animeName">The name of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<AnimeDTO?> GetAnimeAsync(string animeName, CancellationToken cancellationToken);

        /// <summary>
        /// Gets an anime's primary release by Id.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<AnimeReleaseDTO> GetAnimeReleaseAsync(int animeId, CancellationToken cancellationToken)
            => GetAnimeReleaseAsync(animeId, NativeLocaleId, cancellationToken);

        /// <summary>
        /// Gets an anime's release by Id.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="localeId">The Id of the locale to get the release information for.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<AnimeReleaseDTO> GetAnimeReleaseAsync(int animeId, int localeId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets an anime's primary release by name.
        /// </summary>
        /// <param name="animeName">The name of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<AnimeReleaseDTO> GetAnimeReleaseAsync(string animeName, CancellationToken cancellationToken)
            => GetAnimeReleaseAsync(animeName, NativeLocaleId, cancellationToken);

        /// <summary>
        /// Gets an anime's primary release by name.
        /// </summary>
        /// <param name="animeName">The name of the anime.</param>
        /// <param name="localeId">The Id of the locale to get the release information for.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<AnimeReleaseDTO> GetAnimeReleaseAsync(string animeName, int localeId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of anime that are airing or aired this year in the specified season.
        /// </summary>
        /// <param name="season">The season.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>
        /// This will return anime releases that fall into the Japanese release cycle.
        /// </remarks>
        ValueTask<IEnumerable<AnimeDTO>> GetAnimeForSeasonAsync(EAnimeSeason season, CancellationToken cancellationToken)
            => GetAnimeForSeasonAsync(season, DateTime.UtcNow.Year, cancellationToken);

        /// <summary>
        /// Gets a list of anime that are airing or aired in the specified season and year.
        /// </summary>
        /// <param name="season">The season.</param>
        /// <param name="year">The year.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>
        /// This will return anime releases that fall into the Japanese release cycle.
        /// </remarks>
        ValueTask<IEnumerable<AnimeDTO>> GetAnimeForSeasonAsync(EAnimeSeason season, int year, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of anime that are airing or aired this year in the specified season.
        /// </summary>
        /// <param name="season">The season.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>
        /// This will return anime releases that fall into the Japanese release cycle.
        /// </remarks>
        ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleasesForSeasonAsync(EAnimeSeason season, CancellationToken cancellationToken)
            => GetAnimeReleasesForSeasonAsync(season, DateTime.UtcNow.Year, cancellationToken);

        /// <summary>
        /// Gets a list of anime that are airing or aired in the specified season and year.
        /// </summary>
        /// <param name="season">The season.</param>
        /// <param name="year">The year.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>
        /// This will return anime releases that fall into the Japanese release cycle.
        /// </remarks>
        ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleasesForSeasonAsync(EAnimeSeason season, int year, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of anime that have at least the specified minimum user rating.
        /// </summary>
        /// <param name="minimumRating">The minimum user rating.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>Non-native anime releases (dubs) do not have their own ratings. The ratings returned are for the native release.</remarks>
        ValueTask<IEnumerable<AnimeDTO>> GetAnimeWithRatingAsync(byte minimumRating, CancellationToken cancellationToken)
            => GetAnimeWithRatingAsync(minimumRating, 100, cancellationToken);

        /// <summary>
        /// Gets a list of anime that have at least the specified minimum and maximum user rating.
        /// </summary>
        /// <param name="minimumRating">The minimum user rating.</param>
        /// <param name="maximumRating">The maximum user rating.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>Non-native anime releases (dubs) do not have their own ratings. The ratings returned are for the native release.</remarks>
        ValueTask<IEnumerable<AnimeDTO>> GetAnimeWithRatingAsync(byte minimumRating, byte maximumRating, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of anime that have at least the specified minimum user rating.
        /// </summary>
        /// <param name="minimumRating">The minimum user rating.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>
        /// <para>
        /// Non-native anime releases (dubs) do not have their own ratings. The ratings returned are for the native release.
        /// </para>
        /// <para>
        /// The release returned is for the native release.
        /// </para>
        /// </remarks>
        ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseWithRatingAsync(byte minimumRating, CancellationToken cancellationToken)
            => GetAnimeReleaseWithRatingAsync(minimumRating, 100, cancellationToken);

        /// <summary>
        /// Gets a list of anime that have at least the specified minimum user rating.
        /// </summary>
        /// <param name="minimumRating">The minimum user rating.</param>
        /// <param name="maximumRating">The maximum user rating.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>
        /// <para>
        /// Non-native anime releases (dubs) do not have their own ratings. The ratings returned are for the native release.
        /// </para>
        /// <para>
        /// The release returned is for the native release.
        /// </para>
        /// </remarks>
        ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseWithRatingAsync(byte minimumRating, byte maximumRating, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of anime releases for all locales that started airing between <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate">The earliest the first episode aired. If <see langword="null"/>, the start date does not have a lower cap.</param>
        /// <param name="endDate">The latest the first episode aired. If <see langword="null"/>, the end date does not have an upper cap.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByStartDateAsync(DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of anime releases for the specified locale that started airing between <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate">The earliest the first episode aired. If <see langword="null"/>, the start date does not have a lower cap.</param>
        /// <param name="endDate">The latest the first episode aired. If <see langword="null"/>, the end date does not have an upper cap.</param>
        /// <param name="localeId">The Id of the locale to filter releases by.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByStartDateAsync(DateOnly? startDate, DateOnly? endDate, int localeId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of anime releases for all locales that stopped airing between <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate">The earliest the last episode aired. If <see langword="null"/>, the start date does not have a lower cap.</param>
        /// <param name="endDate">The latest the last episode aired. If <see langword="null"/>, the end date does not have an upper cap.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByEndDateAsync(DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of anime releases for the specified locale that stopped airing between <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate">The earliest the last episode aired. If <see langword="null"/>, the start date does not have a lower cap.</param>
        /// <param name="endDate">The latest the last episode aired. If <see langword="null"/>, the end date does not have an upper cap.</param>
        /// <param name="localeId">The Id of the locale to filter releases by.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByEndDateAsync(DateOnly? startDate, DateOnly? endDate, int localeId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of anime releases for all locales that started and stopped airing between <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate">The earliest the first episode aired. If <see langword="null"/>, the start date does not have a lower cap.</param>
        /// <param name="endDate">The latest the last episode aired. If <see langword="null"/>, the end date does not have an upper cap.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByDateAsync(DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of anime releases for the specified locale that started and stopped airing between <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate">The earliest the first episode aired. If <see langword="null"/>, the start date does not have a lower cap.</param>
        /// <param name="endDate">The latest the last episode aired. If <see langword="null"/>, the end date does not have an upper cap.</param>
        /// <param name="localeId">The Id of the locale to filter releases by.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<IEnumerable<AnimeReleaseDTO>> GetAnimeReleaseByDateAsync(DateOnly? startDate, DateOnly? endDate, int localeId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the name of an anime.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The name of the anime.</returns>
        ValueTask<MediaNameDTO?> GetAnimeNameAsync(int animeId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the name of an anime.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="localeId">The Id of the locale to get the anime name in.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<string?> GetAnimeNameAsync(int animeId, int localeId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the category of an anime.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<EAnimeCategory?> GetAnimeCategory(int animeId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the category of an anime.
        /// </summary>
        /// <param name="animeName">The name of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<EAnimeCategory?> GetAnimeCategory(string animeName, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the season an anime aired.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>
        /// This will return the season the native release falls into if it released in the Japanese release cycle.
        /// </remarks>
        ValueTask<EAnimeSeason?> GetAnimeSeason(int animeId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the season an anime aired.
        /// </summary>
        /// <param name="animeName">The name of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>
        /// This will return the season the native release falls into if it released in the Japanese release cycle.
        /// </remarks>
        ValueTask<EAnimeSeason?> GetAnimeSeason(string animeName, CancellationToken cancellationToken);

        /// <summary>
        /// Returns the synopsis of an anime in English.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<string?> GetAnimeSynopsis(int animeId, CancellationToken cancellationToken)
            => GetAnimeSynopsis(animeId, NativeLocaleId, cancellationToken);

        /// <summary>
        /// Returns the synopsis of an anime in the specified locale.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<string?> GetAnimeSynopsis(int animeId, int localeId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns the synopsis of an anime in English.
        /// </summary>
        /// <param name="animeName">The name of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<string?> GetAnimeSynopsis(string animeName, CancellationToken cancellationToken)
            => GetAnimeSynopsis(animeName, NativeLocaleId, cancellationToken);

        /// <summary>
        /// Returns the synopsis of an anime in the specified locale.
        /// </summary>
        /// <param name="animeName">The name of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<string?> GetAnimeSynopsis(string animeName, int localeId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns the status of the native release of an anime.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<EMediaStatus> GetAnimeStatus(int animeId, CancellationToken cancellationToken)
            => GetAnimeStatus(animeId, NativeLocaleId, cancellationToken);

        /// <summary>
        /// Returns the status of the release of an anime.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="localeId">The Id of the locale of the release.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<EMediaStatus> GetAnimeStatus(int animeId, int localeId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns the status of the native release of an anime.
        /// </summary>
        /// <param name="animeName">The name of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<EMediaStatus> GetAnimeStatus(string animeName, CancellationToken cancellationToken)
            => GetAnimeStatus(animeName, NativeLocaleId, cancellationToken);

        /// <summary>
        /// Returns the status of the release of an anime.
        /// </summary>
        /// <param name="animeName">The name of the anime.</param>
        /// <param name="localeId">The Id of the locale of the release.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<EMediaStatus> GetAnimeStatus(string animeName, int localeId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns the age rating of the native release of an anime.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<EAnimeAgeRating?> GetAnimeAgeRating(int animeId, CancellationToken cancellationToken)
            => GetAnimeAgeRating(animeId, NativeLocaleId, cancellationToken);

        /// <summary>
        /// Returns the age rating of the native release of an anime.
        /// </summary>
        /// <param name="animeId">The Id of the anime.</param>
        /// <param name="localeId">The Id of the locale of the release.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<EAnimeAgeRating?> GetAnimeAgeRating(int animeId, int localeId, CancellationToken cancellationToken);

        /// <summary>
        /// Returns the age rating of the native release of an anime.
        /// </summary>
        /// <param name="animeName">The name of the anime.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<EAnimeAgeRating?> GetAnimeAgeRating(string animeName, CancellationToken cancellationToken)
            => GetAnimeAgeRating(animeName, NativeLocaleId, cancellationToken);

        /// <summary>
        /// Returns the age rating of the native release of an anime.
        /// </summary>
        /// <param name="animeName">The name of the anime.</param>
        /// <param name="localeId">The Id of the locale of the release.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        ValueTask<EAnimeAgeRating?> GetAnimeAgeRating(string animeName, int localeId, CancellationToken cancellationToken);
    }
}
