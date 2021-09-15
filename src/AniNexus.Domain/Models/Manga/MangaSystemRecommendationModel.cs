using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models an manga recommendation made by AniNexus.
/// </summary>
/// <remarks>
/// System recommendations intentionally do not have the ability to
/// specify a reason.
/// </remarks>
public class MangaSystemRecommendationModel : IEntityTypeConfiguration<MangaSystemRecommendationModel>
{
    /// <summary>
    /// The Id of the manga.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    /// <summary>
    /// The Id of the manga being recommended.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaRecommendationId { get; set; }

    /// <summary>
    /// The order in which the recommendation is listed among other AniNexus recommendations for
    /// this manga.
    /// </summary>
    /// <remarks>
    /// A lower order will be listed first.
    /// </remarks>
    public byte Order { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The manga the recommendation is based off of.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The manga being recommended as similar to <see cref="Manga"/>.
    /// </summary>
    public MangaModel Recommendation { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaSystemRecommendationModel> builder)
    {
        builder.ToTable("MangaSysRec");

        builder.HasKey(m => new { m.MangaId, m.MangaRecommendationId });

        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Manga).WithMany().HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Recommendation).WithMany().HasForeignKey(m => m.MangaRecommendationId).IsRequired().OnDelete(DeleteBehavior.NoAction);

        // Justification - this is kind of a map, so the navigation properties are always included. In addition, recommendations are
        // generally displayed to a user and they need the name of both manga.
        builder.Navigation(m => m.Manga).AutoInclude();
        builder.Navigation(m => m.Recommendation).AutoInclude();

        builder.Property(m => m.MangaRecommendationId).HasColumnName("MangaRecId");
        builder.Property(m => m.Order).HasComment("The order in which the recommendation will be listed. Lower order will be listed first.").HasDefaultValue(10);
    }
}
