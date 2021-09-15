
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping that associates an anime with a series.
/// </summary>
public class MediaSeriesAnimeMapModel : IEntityTypeConfiguration<MediaSeriesAnimeMapModel>
{
    /// <summary>
    /// The series/IP Id.
    /// </summary>
    /// <seealso cref="MediaSeriesModel"/>
    public int SeriesId { get; set; }

    /// <summary>
    /// The anime Id.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The series.
    /// </summary>
    public MediaSeriesModel Series { get; set; } = default!;

    /// <summary>
    /// The anime.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaSeriesAnimeMapModel> builder)
    {
        builder.ToTable("MediaSeriesAnimeMap");

        builder.HasKey(m => new { m.SeriesId, m.AnimeId });
        builder.HasIndex(m => m.AnimeId);

        builder.HasOne(m => m.Series).WithMany(m => m.Anime).HasForeignKey(m => m.SeriesId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Anime).WithMany(m => m.Series).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Series).AutoInclude();
        builder.Navigation(m => m.Anime).AutoInclude();
    }
}
