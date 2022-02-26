namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a theme.
/// </summary>
public sealed class ThemeEntity : Entity<ThemeEntity, short>, IHasSoftDelete, IHasTranslations<ThemeTranslationEntity, ThemeEntity, short>
{
    /// <summary>
    /// The English name of the theme.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }

    /// <inheritdoc/>
    public IList<ThemeTranslationEntity> Translations { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<ThemeEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => m.Name).IsUnique();
        // 2. Navigation properties
        // 3. Propery specification
        builder.Property(m => m.Name).HasComment("The name of the theme, for example \"Mecha\" or \"School\".").HasMaxLength(64);
        // 4. Other
    }

    /// <inheritdoc/>
    protected override IEnumerable<ThemeEntity> GetSeedData()
    {
        return new[]
        {
            new ThemeEntity { Name = "Demons" },
            new ThemeEntity { Name = "Game" },
            new ThemeEntity { Name = "Harem" },
            new ThemeEntity { Name = "Historical" },
            new ThemeEntity { Name = "Magic Girl" },
            new ThemeEntity { Name = "Martial Arts" },
            new ThemeEntity { Name = "Mecha" },
            new ThemeEntity { Name = "Military" },
            new ThemeEntity { Name = "Music" },
            new ThemeEntity { Name = "Paranormal" },
            new ThemeEntity { Name = "Parody" },
            new ThemeEntity { Name = "Psychological" },
            new ThemeEntity { Name = "Samurai" },
            new ThemeEntity { Name = "School" },
            new ThemeEntity { Name = "Space" },
            new ThemeEntity { Name = "Super Powers" },
            new ThemeEntity { Name = "Vampires" },
        };
    }
}

/// <summary>
/// Models a translation for a theme.
/// </summary>
public sealed class ThemeTranslationEntity : TranslationEntity<ThemeTranslationEntity, ThemeEntity, short>
{
}
