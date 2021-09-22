using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models an anime recommendation made by a user.
/// </summary>
public class AnimeUserRecommendationModel : IHasSoftDelete, IEntityTypeConfiguration<AnimeUserRecommendationModel>
{
    /// <summary>
    /// The Id of the recommendation.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The Id of the anime the recommendation is based on.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the user who made the recommendation.
    /// </summary>
    /// <seealso cref="UserModel"/>
    public Guid UserId { get; set; } = default!;

    /// <summary>
    /// The Id of the anime being recommended.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeRecommendationId { get; set; }

    /// <summary>
    /// The reason they recommend the anime.
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
    /// The user who recommended the anime.
    /// </summary>
    public UserModel User { get; set; } = default!;

    /// <summary>
    /// The anime the recommendation is based on.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The anime being recommended as similar to <see cref="Anime"/>.
    /// </summary>
    public AnimeModel Recommendation { get; set; } = default!;

    /// <summary>
    /// The votes of users who agree or disagree with the recommendation.
    /// </summary>
    public IList<AnimeUserRecommendationVoteModel> Votes { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeUserRecommendationModel> builder)
    {
        builder.ToTable("AnimeUserRec");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => new { m.AnimeId, m.UserId });
        builder.HasIndex(m => m.UserId);
        builder.HasIndex(m => m.AnimeRecommendationId);

        // Many third party trackers will keep user reviews when the user has been deleted,
        // assigning "Deleted" or the like as the reviewer's name. We will not be doing this.
        // We will try and respect the right to be forgotten and remove their recommendation.
        builder.HasOne(m => m.User).WithMany().HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Anime).WithMany().HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Recommendation).WithMany().HasForeignKey(m => m.AnimeRecommendationId).IsRequired().OnDelete(DeleteBehavior.NoAction);

        // Justification - this is kind of a map, so the navigation properties are always included. In addition, recommendations are
        // generally displayed to a user and they need the name of both anime. The user's name is also displayed with the recommendation.
        builder.Navigation(m => m.User).AutoInclude();
        builder.Navigation(m => m.Anime).AutoInclude();
        builder.Navigation(m => m.Recommendation).AutoInclude();

        builder.Property(m => m.Reason).HasComment("The reason why this user recommends the anime.").HasMaxLength(1000).UseCollation(Collation.Japanese);
    }
}
