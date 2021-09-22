using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a user's review of a manga.
/// </summary>
public class MangaReviewModel : IHasAudit, IHasSoftDelete, IEntityTypeConfiguration<MangaReviewModel>
{
    /// <summary>
    /// The Id of the review.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The Id of the user who wrote the review.
    /// </summary>
    /// <seealso cref="UserModel"/>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the manga being reviewed.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    /// <summary>
    /// Whether the user recommends the manga.
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
    /// The manga being reviewed.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The user who wrote the review.
    /// </summary>
    public UserModel User { get; set; } = default!;

    /// <summary>
    /// A collection of votes that affirm or deny the review.
    /// </summary>
    public IList<MangaReviewVoteModel> Votes { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaReviewModel> builder)
    {
        builder.ToTable("MangaReview");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.UserId);
        builder.HasIndex(m => m.MangaId);

        builder.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Manga).WithMany(m => m.Reviews).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - when accessing reviews we generally need the name of the person who posted it
        // and how many votes it has.
        builder.Navigation(m => m.User).AutoInclude();
        builder.Navigation(m => m.Votes).AutoInclude();

        builder.Property(m => m.Recommend).HasComment("Whether the user recommends the manga.");
        builder.Property(m => m.Review).HasComment("The review content.").HasMaxLength(2500).UseCollation(Collation.Japanese);
    }
}
