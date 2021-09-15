#if SONGMODEL

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping that associates a song with a series.
/// </summary>
public class MediaSeriesSongMapModel : IEntityConfiguration<MediaSeriesSongMapModel>
{
    /// <summary>
    /// The series/IP Id.
    /// </summary>
    /// <seealso cref="MediaSeriesModel"/>
    public int SeriesId { get; set; }

    /// <summary>
    /// The song Id.
    /// </summary>
    /// <seealso cref="SongModel"/>
    public int SongId { get; set; }

#region Navigation Properties
    /// <summary>
    /// The series.
    /// </summary>
    public MediaSeriesModel Series { get; set; } = default!;

    /// <summary>
    /// The song.
    /// </summary>
    public SongModel Song { get; set; } = default!;
#endregion

    public static void Configure(EntityTypeBuilder<MediaSeriesSongMapModel> builder)
    {
        builder.ToTable("MediaSeriesSongMap");

        builder.HasKey(m => new { m.SeriesId, m.SongId });
        builder.HasIndex(m => m.SongId);

        builder.HasOne(m => m.Series).WithMany(m => m.Songs).HasForeignKey(m => m.SeriesId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Song).WithMany(m => m.Series).HasForeignKey(m => m.SongId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                
        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Series).AutoInclude();
        builder.Navigation(m => m.Song).AutoInclude();
    }
}
#endif