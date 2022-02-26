namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a user's agreement or disagreement over a user recommendation.
/// </summary>
public class AnimeUserRecommendationVoteEntity : Entity<AnimeUserRecommendationVoteEntity>
{
    /// <summary>
    /// The Id of the user this vote belongs to.
    /// </summary>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the recommendation this vote is associated with.
    /// </summary>
    public Guid RecommendationId { get; set; }

    /// <summary>
    /// Whether the user agrees with the recommendation.
    /// </summary>
    public bool Agrees { get; set; }

    /// <inheritdoc/>
    protected override string GetTableName()
    {
        return "AnimeUserRecVote";
    }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeUserRecommendationVoteEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.UserId, m.RecommendationId }).IsUnique();
        builder.HasIndex(m => m.RecommendationId);
        // 2. Navigation properties
        // Many third party trackers will keep user reviews when the user has been deleted,
        // assigning "Deleted" or the like as the reviewer's name. We will not be doing this.
        // We will try and respect the right to be forgotten and remove their vote.
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths.
        builder.HasOne<UserEntity>().WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne<AnimeUserRecommendationEntity>().WithMany(m => m.Votes).HasForeignKey(m => m.RecommendationId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        builder.Property(m => m.Agrees).HasComment("Whether this user agrees with the recommendation.");
        // 4. Other
    }
}
