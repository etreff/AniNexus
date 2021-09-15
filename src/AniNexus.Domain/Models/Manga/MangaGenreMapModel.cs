using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping of genres and an manga.
/// </summary>
public class MangaGenreMapModel : IEntityTypeConfiguration<MangaGenreMapModel>
{
    /// <summary>
    /// The manga Id.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    /// <summary>
    /// The genre Id.
    /// </summary>
    /// <seealso cref="MediaGenreModel"/>
    public int GenreId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The manga.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The genre.
    /// </summary>
    public MediaGenreModel Genre { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MangaGenreMapModel> builder)
    {
        builder.ToTable("MangaGenreMap");

        builder.HasKey(m => new { m.MangaId, m.GenreId });
        builder.HasIndex(m => m.GenreId);

        builder.HasOne(m => m.Manga).WithMany(m => m.Genres).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Genre).WithMany(m => m.Manga).HasForeignKey(m => m.GenreId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Manga).AutoInclude();
        builder.Navigation(m => m.Genre).AutoInclude();
    }
}
