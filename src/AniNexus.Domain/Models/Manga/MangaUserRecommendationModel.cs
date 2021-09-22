using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models an manga recommendation made by a user.
/// </summary>
public class MangaUserRecommendationModel : IHasSoftDelete, IEntityTypeConfiguration<MangaUserRecommendationModel>
{
    /// <summary>
    /// The Id of the recommendation.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The Id of the manga.
    /// </summary>
    public int MangaId { get; set; }

    /// <summary>
    /// The Id of the user who made the recommendation.
    /// </summary>
    /// <seealso cref="UserModel"/>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the manga being recommended.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaRecommendationId { get; set; }

    /// <summary>
    /// The reason they recommend the manga.
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
    /// The user who recommended the manga.
    /// </summary>
    public UserModel User { get; set; } = default!;

    /// <summary>
    /// The manga the recommendation is based on.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The manga being recommended as similar to <see cref="Manga"/>.
    /// </summary>
    public MangaModel Recommendation { get; set; } = default!;

    /// <summary>
    /// The votes of users who agree or disagree with the recommendation.
    /// </summary>
    public IList<MangaUserRecommendationVoteModel> Votes { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaUserRecommendationModel> builder)
    {
        builder.ToTable("MangaUserRec");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => new { m.MangaId, m.UserId });
        builder.HasIndex(m => m.UserId);
        builder.HasIndex(m => m.MangaRecommendationId);

        // Many third party trackers will keep user reviews when the user has been deleted,
        // assigning "Deleted" or the like as the reviewer's name. We will not be doing this.
        // We will try and respect the right to be forgotten and remove their recommendation.
        builder.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Manga).WithMany().HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Recommendation).WithMany().HasForeignKey(m => m.MangaRecommendationId).IsRequired().OnDelete(DeleteBehavior.NoAction);

        // Justification - this is kind of a map, so the navigation properties are always included. In addition, recommendations are
        // generally displayed to a user and they need the name of both manga. The user's name is also displayed with the recommendation.
        builder.Navigation(m => m.User).AutoInclude();
        builder.Navigation(m => m.Manga).AutoInclude();
        builder.Navigation(m => m.Recommendation).AutoInclude();

        builder.Property(m => m.Reason).HasComment("The reason why this user recommends the manga.").HasMaxLength(1000).UseCollation(Collation.Japanese);
    }
}
