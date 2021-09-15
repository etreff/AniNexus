using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a media genre.
/// </summary>
public class MediaGenreModel : IHasAudit, IHasSoftDelete, IEntityTypeConfiguration<MediaGenreModel>
{
    /// <summary>
    /// The Id of the genre.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The English name of the genre.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Whether the genre is broadly NSFW.
    /// </summary>
    /// <remarks>
    /// Hentai is always NSFW. Ecchi is not NSFW on a broad level, but it might
    /// be depending on more granular tags.
    /// </remarks>
    /// <seealso cref="MediaTagModel"/>
    public bool IsNSFW { get; set; }

    /// <summary>
    /// Whether the genre is broadly considered to contain gore.
    /// </summary>
    /// <remarks>
    /// If a category does not contain gore by definition, use <see cref="MediaTagModel"/>
    /// to flag a piece of media as containing gore on a more granular basis.
    /// </remarks>
    /// <seealso cref="MediaTagModel"/>
    public bool IsGore { get; set; }

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
    /// The anime that contains this genre.
    /// </summary>
    public IList<AnimeGenreMapModel> Anime { get; set; } = default!;

    /// <summary>
    /// The games that contains this genre.
    /// </summary>
    public IList<GameGenreMapModel> Games { get; set; } = default!;

    /// <summary>
    /// The manga that contains this genre.
    /// </summary>
    public IList<MangaGenreMapModel> Manga { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaGenreModel> builder)
    {
        builder.ToTable("MediaGenre");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.Name).IsUnique();

        builder.HasData(
            new MediaGenreModel { Id = 1, Name = "4-Koma" },
            new MediaGenreModel { Id = 2, Name = "Adventure" },
            new MediaGenreModel { Id = 3, Name = "Comedy" },
            new MediaGenreModel { Id = 4, Name = "Daily Life" },
            new MediaGenreModel { Id = 5, Name = "High School" },
            new MediaGenreModel { Id = 6, Name = "Manga" },
            new MediaGenreModel { Id = 7, Name = "Romance" },
            new MediaGenreModel { Id = 8, Name = "School" },
            new MediaGenreModel { Id = 9, Name = "Shounen" },
            new MediaGenreModel { Id = 10, Name = "Shoujo" },
            new MediaGenreModel { Id = 11, Name = "Sports" },
            new MediaGenreModel { Id = 12, Name = "Mecha" },
            new MediaGenreModel { Id = 13, Name = "Fantasy" },
            new MediaGenreModel { Id = 14, Name = "Music" },
            new MediaGenreModel { Id = 15, Name = "Magic Girl" });

        builder.Property(m => m.Name).HasComment("The name of the genre, for example \"Action\" or \"Fantasy\".").HasMaxLength(32);
        builder.Property(m => m.IsNSFW).HasComment("Whether this tag marks an entity as NSFW.");
        builder.Property(m => m.IsGore).HasComment("Whether this tag marks an entity as containing gore.");
    }
}
