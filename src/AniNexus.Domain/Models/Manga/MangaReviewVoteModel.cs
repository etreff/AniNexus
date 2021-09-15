using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a user's agreement or disagreement over a review.
/// </summary>
public class MangaReviewVoteModel : IEntityTypeConfiguration<MangaReviewVoteModel>
{
    /// <summary>
    /// The Id of the vote.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The Id of the user.
    /// </summary>
    /// <seealso cref="ApplicationUserModel"/>
    public string UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the review this vote is associated with.
    /// </summary>
    /// <seealso cref="MangaReviewModel"/>
    public int ReviewId { get; set; }

    /// <summary>
    /// Whether the user agrees with the review.
    /// </summary>
    public bool Agrees { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The user that this vote belongs to.
    /// </summary>
    public ApplicationUserModel User { get; set; } = default!;

    /// <summary>
    /// The review this vote is associated with.
    /// </summary>
    public MangaReviewModel Review { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaReviewVoteModel> builder)
    {
        builder.ToTable("MangaReviewVote");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => new { m.UserId, m.ReviewId }).IsUnique();
        builder.HasIndex(m => m.ReviewId);

        // Many third party trackers will keep user reviews when the user has been deleted,
        // assigning "Deleted" or the like as the reviewer's name. We will not be doing this.
        // We will try and respect the right to be forgotten and remove their vote.
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths.
        builder.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Review).WithMany(m => m.Votes).HasForeignKey(m => m.ReviewId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.Agrees).HasComment("Whether this user agrees with the review.");
    }
}
