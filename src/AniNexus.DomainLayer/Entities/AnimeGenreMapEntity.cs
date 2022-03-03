namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a mapping of genres and an anime.
/// </summary>
public class AnimeGenreMapEntity : Entity<AnimeGenreMapEntity, int>
{
    /// <summary>
    /// The Id of the anime the genre applies to.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The Id of the genre to apply to the anime.
    /// </summary>
    public byte GenreId { get; set; }

    /// <summary>
    /// The anime the genre applies to.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The genre to apply to the anime.
    /// </summary>
    public VideoGenreEntity Genre { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeGenreMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.AnimeId, m.GenreId }).IsUnique();
        builder.HasIndex(m => m.GenreId);
        // 2. Navigation properties
        builder.HasOne(m => m.Anime).WithMany(m => m.Genres).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Genre).WithMany(m => m.Anime).HasForeignKey(m => m.GenreId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        // 4. Other
        builder.HasQueryFilter(m => !m.Anime.IsSoftDeleted && !m.Genre.IsSoftDeleted);
    }
}
