using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a parent series or intellectual property.
/// </summary>
public class MediaSeriesModel : IHasAudit, IHasRowVersion, IHasSoftDelete, IEntityTypeConfiguration<MediaSeriesModel>
{
    /// <summary>
    /// The Id of the series.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// A synopsis of the series.
    /// </summary>
    public string? Synopsis { get; set; }

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
    /// The row version.
    /// </summary>
    public byte[] RowVersion { get; set; } = default!;

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }
    #endregion

    #region Navigation Properties
    /// <summary>
    /// The names of the series.
    /// </summary>
    public IList<NameModel> Names { get; set; } = default!;

    /// <summary>
    /// Models a name of a media series.
    /// </summary>
    public class NameModel
    {
        /// <summary>
        /// The name in the native language.
        /// </summary>
        public string? NativeName { get; set; }

        /// <summary>
        /// The romanization of the native name.
        /// </summary>
        public string? RomajiName { get; set; }

        /// <summary>
        /// The name in English.
        /// </summary>
        public string? EnglishName { get; set; }

        /// <summary>
        /// Whether the name is the primary name.
        /// </summary>
        public bool IsPrimary { get; set; }
    }

    /// <summary>
    /// Anime that are associated with this series.
    /// </summary>
    public IList<MediaSeriesAnimeMapModel> Anime { get; set; } = default!;

    /// <summary>
    /// Games that are associated with this series.
    /// </summary>
    public IList<MediaSeriesGameMapModel> Games { get; set; } = default!;

    /// <summary>
    /// Manga that are associated with this series.
    /// </summary>
    public IList<MediaSeriesMangaMapModel> Manga { get; set; } = default!;
#if SONGMODEL
    /// <summary>
    /// Albums that are associated with this series.
    /// </summary>
    public IList<MediaSeriesAlbumMapModel> Albums { get; set; } = default!;

    /// <summary>
    /// Songs that are associated with this series.
    /// </summary>
    public IList<MediaSeriesSongMapModel> Songs { get; set; } = default!;
#endif
    #endregion

    public void Configure(EntityTypeBuilder<MediaSeriesModel> builder)
    {
        builder.ToTable("MediaSeries");

        builder.HasKey(m => m.Id);

        builder.OwnsMany(m => m.Names, name =>
        {
            name.ToTable("MediaSeriesName");

            name.Property(m => m.NativeName).HasComment("The native name.").HasColumnName(nameof(NameModel.NativeName));
            name.Property(m => m.RomajiName).HasComment("The romanization of the native name.").HasColumnName(nameof(NameModel.RomajiName));
            name.Property(m => m.EnglishName).HasComment("The name in English.").HasColumnName(nameof(NameModel.EnglishName));
            name.Property(m => m.IsPrimary).HasComment("Whether this name is the primary name of the series.").HasColumnName(nameof(NameModel.IsPrimary));
        });

        builder.Property(m => m.Synopsis).HasComment("A synopsis of the media series.").HasMaxLength(1250);
    }
}
