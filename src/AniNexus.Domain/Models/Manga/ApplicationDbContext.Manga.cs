using AniNexus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain;

public partial class ApplicationDbContext
{
    /// <summary>
    /// A collection of manga chapters.
    /// </summary>
    public DbSet<MangaChapterModel> MangaChapters => Set<MangaChapterModel>();

    /// <summary>
    /// A collection of user manga list entries.
    /// </summary>
    public DbSet<MangaListEntryModel> MangaListEntries => Set<MangaListEntryModel>();

    /// <summary>
    /// A collection of manga.
    /// </summary>
    public DbSet<MangaModel> Manga => Set<MangaModel>();

    /// <summary>
    /// A collection of manga releases.
    /// </summary>
    public DbSet<MangaReleaseModel> MangaReleases => Set<MangaReleaseModel>();

    /// <summary>
    /// A collection of manga reviews made by users.
    /// </summary>
    public DbSet<MangaReviewModel> MangaReviews => Set<MangaReviewModel>();

    /// <summary>
    /// A collection of votes associated with a manga review.
    /// </summary>
    /// <seealso cref="MangaReviews"/>
    public DbSet<MangaReviewVoteModel> MangaReviewVotes => Set<MangaReviewVoteModel>();

    /// <summary>
    /// A collection of manga recommendations made by AniNexus staff.
    /// </summary>
    public DbSet<MangaSystemRecommendationModel> MangaSystemRecommendations => Set<MangaSystemRecommendationModel>();

    /// <summary>
    /// A collection of manga recommendations made by users.
    /// </summary>
    public DbSet<MangaUserRecommendationModel> MangaUserRecommendations => Set<MangaUserRecommendationModel>();

    /// <summary>
    /// A collection of votes associated with a manga recommendation.
    /// </summary>
    /// <seealso cref="MangaUserRecommendations"/>
    public DbSet<MangaUserRecommendationVoteModel> MangaUserRecommendationVotes => Set<MangaUserRecommendationVoteModel>();

    /// <summary>
    /// A collection of manga volumes.
    /// </summary>
    public DbSet<MangaVolumeModel> MangaVolumes => Set<MangaVolumeModel>();

}
