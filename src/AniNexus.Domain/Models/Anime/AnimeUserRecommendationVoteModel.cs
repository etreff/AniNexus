using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a user's agreement or disagreement over a user recommendation.
/// </summary>
public class AnimeUserRecommendationVoteModel : IEntityTypeConfiguration<AnimeUserRecommendationVoteModel>
{
    /// <summary>
    /// The Id of the user.
    /// </summary>
    /// <seealso cref="UserModel"/>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the recommendation this vote is associated with.
    /// </summary>
    /// <seealso cref="AnimeUserRecommendationModel"/>
    public int RecommendationId { get; set; }

    /// <summary>
    /// Whether the user agrees with the recommendation.
    /// </summary>
    public bool Agrees { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The user.
    /// </summary>
    public UserModel User { get; set; } = default!;

    /// <summary>
    /// The recommendation this vote is associated with.
    /// </summary>
    public AnimeUserRecommendationModel Recommendation { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeUserRecommendationVoteModel> builder)
    {
        builder.ToTable("AnimeUserRecVote");

        builder.HasKey(m => new { m.UserId, m.RecommendationId });
        builder.HasIndex(m => m.RecommendationId);

        // Many third party trackers will keep user reviews when the user has been deleted,
        // assigning "Deleted" or the like as the reviewer's name. We will not be doing this.
        // We will try and respect the right to be forgotten and remove their vote.
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths.
        builder.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Recommendation).WithMany(m => m.Votes).HasForeignKey(m => m.RecommendationId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.Agrees).HasComment("Whether this user agrees with the recommendation.");
    }
}
