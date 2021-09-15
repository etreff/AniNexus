using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models an anime recommendation made by AniNexus.
/// </summary>
/// <remarks>
/// System recommendations intentionally do not have the ability to specify a reason.
/// </remarks>
public class AnimeSystemRecommendationModel : IEntityTypeConfiguration<AnimeSystemRecommendationModel>
{
    /// <summary>
    /// The Id of the anime the recommendation is based off of.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the anime being recommended.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeRecommendationId { get; set; }

    /// <summary>
    /// The order in which the recommendation is listed among other AniNexus recommendations for
    /// this anime.
    /// </summary>
    /// <remarks>
    /// A lower order will be listed first.
    /// </remarks>
    public byte Order { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The anime the recommendation is based off of.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The anime being recommended as similar to <see cref="Anime"/>.
    /// </summary>
    public AnimeModel Recommendation { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeSystemRecommendationModel> builder)
    {
        builder.ToTable("AnimeSysRec");

        builder.HasKey(m => new { m.AnimeId, m.AnimeRecommendationId });
        builder.HasIndex(m => m.AnimeRecommendationId);

        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Anime).WithMany().HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Recommendation).WithMany().HasForeignKey(m => m.AnimeRecommendationId).IsRequired().OnDelete(DeleteBehavior.NoAction);

        // Justification - this is kind of a map, so the navigation properties are always included. In addition, recommendations are
        // generally displayed to a user and they need the name of both anime.
        builder.Navigation(m => m.Anime).AutoInclude();
        builder.Navigation(m => m.Recommendation).AutoInclude();

        builder.Property(m => m.AnimeRecommendationId).HasColumnName("AnimeRecId");
        builder.Property(m => m.Order).HasComment("The order in which the recommendation will be listed. Lower order will be listed first.").HasDefaultValue(10);
    }
}
