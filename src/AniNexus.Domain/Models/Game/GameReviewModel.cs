using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a user's review of a game.
/// </summary>
public class GameReviewModel : IHasAudit, IHasSoftDelete, IEntityTypeConfiguration<GameReviewModel>
{
    /// <summary>
    /// The Id of the review.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The Id of the user who wrote the review.
    /// </summary>
    /// <seealso cref="ApplicationUserModel"/>
    public string UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the game being reviewed.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

    /// <summary>
    /// Whether the user recommends the game.
    /// </summary>
    public bool Recommend { get; set; }

    /// <summary>
    /// The contents of the review.
    /// </summary>
    public string Review { get; set; } = default!;

    #region Interface Properties
    /// <summary>
    /// The UTC date and time this entry was added to the table.
    /// </summary>
    public DateTime DateAdded { get; set; }

    /// <summary>
    /// The UTC date and time this entry was last updated.
    /// </summary>
    public DateTime DateUpdated { get; set; }

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }
    #endregion

    #region Navigation Properties
    /// <summary>
    /// The game being reviewed.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The user who wrote the review.
    /// </summary>
    public ApplicationUserModel User { get; set; } = default!;

    /// <summary>
    /// A collection of votes that affirm or deny the review.
    /// </summary>
    public IList<GameReviewVoteModel> Votes { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<GameReviewModel> builder)
    {
        builder.ToTable("GameReview");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.UserId);
        builder.HasIndex(m => m.GameId);

        builder.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Game).WithMany(m => m.Reviews).HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - when accessing reviews we generally need the name of the person who posted it
        // and how many votes it has.
        builder.Navigation(m => m.User).AutoInclude();
        builder.Navigation(m => m.Votes).AutoInclude();

        builder.Property(m => m.Recommend).HasComment("Whether the user recommends the game.");
        builder.Property(m => m.Review).HasComment("The review content.").HasMaxLength(2500).UseCollation(Collation.Japanese);
    }
}
