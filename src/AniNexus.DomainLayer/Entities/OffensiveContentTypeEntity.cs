namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a type of content that may be triggering or disturbing for an audience.
/// </summary>
public sealed class OffensiveContentTypeEntity : Entity<OffensiveContentTypeEntity, byte>, IHasTranslations<OffensiveContentTypeTranslationEntity, OffensiveContentTypeEntity, byte>
{
    /// <summary>
    /// The name of the trigger.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Whether this content would be considered a spoiler.
    /// </summary>
    public bool IsSpoiler { get; set; }

    /// <summary>
    /// Regions in which this content type is NSFW.
    /// </summary>
    public IList<OffensiveNsfwContentTypeEntity> NsfwRegions { get; set; } = default!;

    /// <summary>
    /// Regions in which this content type is banned.
    /// </summary>
    /// <remarks>
    /// This implies it is also NSFW.
    /// </remarks>
    public IList<OffensiveBannedContentTypeEntity> BannedRegions { get; set; } = default!;

    /// <inheritdoc/>
    public IList<OffensiveContentTypeTranslationEntity> Translations { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<OffensiveContentTypeEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(e => e.Name).IsUnique();
        // 2. Navigation properties
        // 3. Propery specification
        builder.Property(m => m.Name).HasComment("The name of the trigger.");
        builder.Property(m => m.IsSpoiler).HasComment("Whether this content would be considered a spoiler.");
        // 4. Other
    }

    /// <inheritdoc/>
    protected override IEnumerable<OffensiveContentTypeEntity> GetSeedData()
    {
        return new[]
        {
            new OffensiveContentTypeEntity { Name = "Nudity" },
            new OffensiveContentTypeEntity { Name = "Pornography"},
            new OffensiveContentTypeEntity { Name = "Gore" },
            new OffensiveContentTypeEntity { Name = "Violence" },
            new OffensiveContentTypeEntity { Name = "Rape"},
            new OffensiveContentTypeEntity { Name = "Abuse"},
            new OffensiveContentTypeEntity { Name = "Pedophilia"},
            new OffensiveContentTypeEntity { Name = "Cruelty"},
            new OffensiveContentTypeEntity { Name = "Death"},
            new OffensiveContentTypeEntity { Name = "Self Harm"},
            new OffensiveContentTypeEntity { Name = "Suicide"},
            new OffensiveContentTypeEntity { Name = "Incest"},
            new OffensiveContentTypeEntity { Name = "Homosexuality"},
            new OffensiveContentTypeEntity { Name = "Transgender"}
        };
    }
}

/// <summary>
/// Models a translation for an offensive content type.
/// </summary>
public sealed class OffensiveContentTypeTranslationEntity : TranslationEntity<OffensiveContentTypeTranslationEntity, OffensiveContentTypeEntity, byte>
{
}
