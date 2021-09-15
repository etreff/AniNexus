
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping that associates a manga with a series.
/// </summary>
public class MediaSeriesMangaMapModel : IEntityTypeConfiguration<MediaSeriesMangaMapModel>
{
    /// <summary>
    /// The series/IP Id.
    /// </summary>
    /// <seealso cref="MediaSeriesModel"/>
    public int SeriesId { get; set; }

    /// <summary>
    /// The manga Id.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The series.
    /// </summary>
    public MediaSeriesModel Series { get; set; } = default!;

    /// <summary>
    /// The manga.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaSeriesMangaMapModel> builder)
    {
        builder.ToTable("MediaSeriesMangaMap");

        builder.HasKey(m => new { m.SeriesId, m.MangaId });
        builder.HasIndex(m => m.MangaId);

        builder.HasOne(m => m.Series).WithMany(m => m.Manga).HasForeignKey(m => m.SeriesId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Manga).WithMany(m => m.Series).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Series).AutoInclude();
        builder.Navigation(m => m.Manga).AutoInclude();
    }
}
