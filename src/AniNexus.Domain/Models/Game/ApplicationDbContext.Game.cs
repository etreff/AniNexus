using AniNexus.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AniNexus.Domain;

public partial class ApplicationDbContext
{
    /// <summary>
    /// A collection of user game list entries.
    /// </summary>
    public DbSet<GameListEntryModel> GameListEntries => Set<GameListEntryModel>();

    /// <summary>
    /// A collection of game.
    /// </summary>
    public DbSet<GameModel> Games => Set<GameModel>();

    /// <summary>
    /// A collection of game reviews made by users.
    /// </summary>
    public DbSet<GameReviewModel> GameReviews => Set<GameReviewModel>();

    /// <summary>
    /// A collection of votes associated with an game review.
    /// </summary>
    /// <seealso cref="GameReviews"/>
    public DbSet<GameReviewVoteModel> GameReviewVotes => Set<GameReviewVoteModel>();

    /// <summary>
    /// A collection of game recommendations made by AniNexus staff.
    /// </summary>
    public DbSet<GameSystemRecommendationModel> GameSystemRecommendations => Set<GameSystemRecommendationModel>();

    /// <summary>
    /// A collection of game recommendations made by users.
    /// </summary>
    public DbSet<GameUserRecommendationModel> GameUserRecommendations => Set<GameUserRecommendationModel>();

    /// <summary>
    /// A collection of votes associated with an game recommendation.
    /// </summary>
    /// <seealso cref="GameUserRecommendations"/>
    public DbSet<GameUserRecommendationVoteModel> GameUserRecommendationVotes => Set<GameUserRecommendationVoteModel>();
}
