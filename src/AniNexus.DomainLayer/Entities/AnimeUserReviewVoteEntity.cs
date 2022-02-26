namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a user's agreement or disagreement over a review.
/// </summary>
public class AnimeUserReviewVoteEntity : Entity<AnimeUserReviewVoteEntity, long>
{
    /// <summary>
    /// The Id of the user.
    /// </summary>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the review this vote is associated with.
    /// </summary>
    public Guid ReviewId { get; set; }

    /// <summary>
    /// Whether the user agrees with the review.
    /// </summary>
    public bool Agrees { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeUserReviewVoteEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.UserId, m.ReviewId }).IsUnique();
        builder.HasIndex(m => m.ReviewId);
        // 2. Navigation properties
        // Many third party trackers will keep user reviews when the user has been deleted,
        // assigning "Deleted" or the like as the reviewer's name. We will not be doing this.
        // We will try and respect the right to be forgotten and remove their vote.
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths.
        builder.HasOne<UserEntity>().WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne<AnimeUserReviewEntity>().WithMany(m => m.Votes).HasForeignKey(m => m.ReviewId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        builder.Property(m => m.Agrees).HasComment("Whether this user agrees with the review.");
        // 4. Other
    }
}
