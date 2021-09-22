using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models an game recommendation made by a user.
/// </summary>
public class GameUserRecommendationModel : IHasSoftDelete, IEntityTypeConfiguration<GameUserRecommendationModel>
{
    /// <summary>
    /// The Id of the recommendation.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The Id of the game the recommendation is based on.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// The Id of the user who made the recommendation.
    /// </summary>
    /// <seealso cref="UserModel"/>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the game being recommended.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameRecommendationId { get; set; }

    /// <summary>
    /// The reason they recommend the game.
    /// </summary>
    public string Reason { get; set; } = default!;

    #region Interface Properties
    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }
    #endregion

    #region Navigation Properties
    /// <summary>
    /// The user who recommended the game.
    /// </summary>
    public UserModel User { get; set; } = default!;

    /// <summary>
    /// The game the recommendation is based on.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The game being recommended as similar to <see cref="Game"/>.
    /// </summary>
    public GameModel Recommendation { get; set; } = default!;

    /// <summary>
    /// The votes of users who agree or disagree with the recommendation.
    /// </summary>
    public IList<GameUserRecommendationVoteModel> Votes { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GameUserRecommendationModel> builder)
    {
        builder.ToTable("GameUserRec");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => new { m.GameId, m.UserId });
        builder.HasIndex(m => m.UserId);
        builder.HasIndex(m => m.GameRecommendationId);

        // Many third party trackers will keep user reviews when the user has been deleted,
        // assigning "Deleted" or the like as the reviewer's name. We will not be doing this.
        // We will try and respect the right to be forgotten and remove their recommendation.
        builder.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Game).WithMany().HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Recommendation).WithMany().HasForeignKey(m => m.GameRecommendationId).IsRequired().OnDelete(DeleteBehavior.NoAction);

        // Justification - this is kind of a map, so the navigation properties are always included. In addition, recommendations are
        // generally displayed to a user and they need the name of both game. The user's name is also displayed with the recommendation.
        builder.Navigation(m => m.User).AutoInclude();
        builder.Navigation(m => m.Game).AutoInclude();
        builder.Navigation(m => m.Recommendation).AutoInclude();

        builder.Property(m => m.Reason).HasComment("The reason why this user recommends the game.").HasMaxLength(1000).UseCollation(Collation.Japanese);
    }
}
