using AniNexus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain;

public partial class ApplicationDbContext
{
    /// <summary>
    /// Anime that is currently airing.
    /// </summary>
    public DbSet<AnimeReleaseAiringModel> AnimeAiring => Set<AnimeReleaseAiringModel>();

    /// <summary>
    /// A collection of anime episodes.
    /// </summary>
    public DbSet<AnimeEpisodeModel> AnimeEpisodes => Set<AnimeEpisodeModel>();

    /// <summary>
    /// A collection of user anime list entries.
    /// </summary>
    public DbSet<AnimeListEntryModel> AnimeListEntries => Set<AnimeListEntryModel>();

    /// <summary>
    /// A collection of anime.
    /// </summary>
    public DbSet<AnimeModel> Anime => Set<AnimeModel>();

    /// <summary>
    /// A collection of anime releases.
    /// </summary>
    public DbSet<AnimeReleaseModel> AnimeReleases => Set<AnimeReleaseModel>();

    /// <summary>
    /// A collection of anime reviews made by users.
    /// </summary>
    public DbSet<AnimeReviewModel> AnimeReviews => Set<AnimeReviewModel>();

    /// <summary>
    /// A collection of votes associated with an anime review.
    /// </summary>
    /// <seealso cref="AnimeReviews"/>
    public DbSet<AnimeReviewVoteModel> AnimeReviewVotes => Set<AnimeReviewVoteModel>();

    /// <summary>
    /// A collection of anime recommendations made by AniNexus staff.
    /// </summary>
    public DbSet<AnimeSystemRecommendationModel> AnimeSystemRecommendations => Set<AnimeSystemRecommendationModel>();

    /// <summary>
    /// A collection of anime recommendations made by users.
    /// </summary>
    public DbSet<AnimeUserRecommendationModel> AnimeUserRecommendations => Set<AnimeUserRecommendationModel>();

    /// <summary>
    /// A collection of votes associated with an anime recommendation.
    /// </summary>
    /// <seealso cref="AnimeUserRecommendations"/>
    public DbSet<AnimeUserRecommendationVoteModel> AnimeUserRecommendationVotes => Set<AnimeUserRecommendationVoteModel>();
}
