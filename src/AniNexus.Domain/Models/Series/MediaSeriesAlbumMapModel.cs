#if SONGMODEL

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping that associates a album with a series.
/// </summary>
public class MediaSeriesAlbumMapModel : IEntityConfiguration<MediaSeriesAlbumMapModel>
{
    /// <summary>
    /// The series/IP Id.
    /// </summary>
    /// <seealso cref="MediaSeriesModel"/>
    public int SeriesId { get; set; }

    /// <summary>
    /// The album Id.
    /// </summary>
    /// <seealso cref="AlbumModel"/>
    public int AlbumId { get; set; }

#region Navigation Properties
    /// <summary>
    /// The series.
    /// </summary>
    public MediaSeriesModel Series { get; set; } = default!;

    /// <summary>
    /// The album.
    /// </summary>
    public AlbumModel Album { get; set; } = default!;
#endregion

    public static void Configure(EntityTypeBuilder<MediaSeriesAlbumMapModel> builder)
    {
        builder.ToTable("MediaSeriesAlbumMap");

        builder.HasKey(m => new { m.SeriesId, m.AlbumId });
        builder.HasKey(m => m.AlbumId);

        builder.HasOne(m => m.Series).WithMany(m => m.Albums).HasForeignKey(m => m.SeriesId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Album).WithMany(m => m.Series).HasForeignKey(m => m.AlbumId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        
        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Series).AutoInclude();
        builder.Navigation(m => m.Album).AutoInclude();
    }
}

#endif