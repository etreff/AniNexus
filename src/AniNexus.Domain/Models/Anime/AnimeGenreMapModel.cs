using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping of genres and an anime.
/// </summary>
public class AnimeGenreMapModel : IEntityTypeConfiguration<AnimeGenreMapModel>
{
    /// <summary>
    /// The Id of the anime the genre applies to.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the genre to apply to the anime.
    /// </summary>
    /// <seealso cref="MediaGenreModel"/>
    public int GenreId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The anime the genre applies to.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The genre to apply to the anime.
    /// </summary>
    public MediaGenreModel Genre { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<AnimeGenreMapModel> builder)
    {
        builder.ToTable("AnimeGenreMap");

        builder.HasKey(m => new { m.AnimeId, m.GenreId });
        builder.HasIndex(m => m.GenreId);

        builder.HasOne(m => m.Anime).WithMany(m => m.Genres).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Genre).WithMany(m => m.Anime).HasForeignKey(m => m.GenreId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Anime).AutoInclude();
        builder.Navigation(m => m.Genre).AutoInclude();
    }
}
