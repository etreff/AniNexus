namespace AniNexus.Data.Entities;

/// <summary>
/// Models a genre.
/// </summary>
public sealed class VideoGenreEntity : Entity<VideoGenreEntity, byte>, IHasSoftDelete, IHasTranslations<VideoGenreTranslationEntity, VideoGenreEntity, byte>
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
    public IList<VideoGenreTranslationEntity> Translations { get; set; } = default!;

    /// <summary>
    /// Anime that have been tagged with this genre.
    /// </summary>
    public IList<AnimeGenreMapEntity> Anime { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<VideoGenreEntity> builder)
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
    protected override IEnumerable<VideoGenreEntity> GetSeedData()
    {
        return new[]
        {
            new VideoGenreEntity { Name = "4-Koma" },
            new VideoGenreEntity { Name = "Action" },
            new VideoGenreEntity { Name = "Adventure" },
            new VideoGenreEntity { Name = "Boy's Love" },
            new VideoGenreEntity { Name = "Comedy" },
            new VideoGenreEntity { Name = "Cooking" },
            new VideoGenreEntity { Name = "Daily Life" },
            new VideoGenreEntity { Name = "Drama" },
            new VideoGenreEntity { Name = "Ecchi" },
            new VideoGenreEntity { Name = "Fantasy" },
            new VideoGenreEntity { Name = "Girl's Love" },
            new VideoGenreEntity { Name = "Hentai", IsNSFW = true },
            new VideoGenreEntity { Name = "Horror" },
            new VideoGenreEntity { Name = "Mystery" },
            new VideoGenreEntity { Name = "Romance" },
            new VideoGenreEntity { Name = "Science Fiction" },
            new VideoGenreEntity { Name = "Sports" },
            new VideoGenreEntity { Name = "Supernatural" },
            new VideoGenreEntity { Name = "Suspense" }
        };
    }
}

/// <summary>
/// Models a translation for a genre.
/// </summary>
public sealed class VideoGenreTranslationEntity : TranslationEntity<VideoGenreTranslationEntity, VideoGenreEntity, byte>
{
}
