namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a genre.
/// </summary>
public sealed class GenreEntity : Entity<GenreEntity, byte>, IHasSoftDelete, IHasTranslations<GenreTranslationEntity, GenreEntity, byte>
{
    /// <summary>
    /// The English name of the genre.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Whether the genre is broadly NSFW.
    /// </summary>
    /// <remarks>
    /// Hentai is always NSFW. Ecchi is not NSFW on a broad level, but it might be
    /// depending on the region.
    /// </remarks>
    public bool IsNSFW { get; set; }

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }

    /// <inheritdoc/>
    public IList<GenreTranslationEntity> Translations { get; set; } = default!;

    /// <summary>
    /// Anime that have been tagged with this genre.
    /// </summary>
    public IList<AnimeGenreMapEntity> Anime { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<GenreEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => m.Name).IsUnique();
        // 2. Navigation properties
        // 3. Propery specification
        builder.Property(m => m.Name).HasComment("The name of the genre, for example \"Action\" or \"Fantasy\".").HasMaxLength(64);
        builder.Property(m => m.IsNSFW).HasComment("Whether this tag marks an entity as NSFW.");
        // 4. Other
    }

    /// <inheritdoc/>
    protected override IEnumerable<GenreEntity> GetSeedData()
    {
        return new[]
        {
            new GenreEntity { Name = "4-Koma" },
            new GenreEntity { Name = "Action" },
            new GenreEntity { Name = "Adventure" },
            new GenreEntity { Name = "Boy's Love" },
            new GenreEntity { Name = "Comedy" },
            new GenreEntity { Name = "Cooking" },
            new GenreEntity { Name = "Daily Life" },
            new GenreEntity { Name = "Drama" },
            new GenreEntity { Name = "Ecchi" },
            new GenreEntity { Name = "Fantasy" },
            new GenreEntity { Name = "Girl's Love" },
            new GenreEntity { Name = "Hentai", IsNSFW = true },
            new GenreEntity { Name = "Horror" },
            new GenreEntity { Name = "Mystery" },
            new GenreEntity { Name = "Romance" },
            new GenreEntity { Name = "Science Fiction" },
            new GenreEntity { Name = "Sports" },
            new GenreEntity { Name = "Supernatural" },
            new GenreEntity { Name = "Suspense" }
        };
    }
}

/// <summary>
/// Models a translation for a genre.
/// </summary>
public sealed class GenreTranslationEntity : TranslationEntity<GenreTranslationEntity, GenreEntity, byte>
{
}
