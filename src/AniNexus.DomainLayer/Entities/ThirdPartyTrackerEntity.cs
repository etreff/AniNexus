using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a third party tracker.
/// </summary>
public sealed class ThirdPartyTrackerEntity : AuditableEntity<ThirdPartyTrackerEntity>, IHasSoftDelete
{
    /// <summary>
    /// The name of the tracker.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The short name or alias of the tracker.
    /// </summary>
    public string? ShortName { get; set; }

    /// <summary>
    /// A URL to the tracker's website.
    /// </summary>
    public string? Url { get; set; } = default!;

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<ThirdPartyTrackerEntity> builder)
    {
        // 1. Index specification
        // 2. Navigation properties
        // 3. Propery specification
        builder.Property(m => m.Name).HasComment("The name of the third party tracker.").HasMaxLength(64).UseCollation(Collation.Japanese);
        builder.Property(m => m.ShortName).HasComment("The short name or alias of the third party tracker.").HasMaxLength(16).UseCollation(Collation.Japanese);
        builder.Property(m => m.Url).HasComment("The URL of the third party tracker's website homepage.").HasMaxLength(128);
        // 4. Other
    }

    /// <inheritdoc/>
    protected override IEnumerable<ThirdPartyTrackerEntity> GetSeedData()
    {
        return new[]
        {
            new ThirdPartyTrackerEntity { Name = "AniDB", Url = "https://anidb.net/" },
            new ThirdPartyTrackerEntity { Name = "TheTVDB", Url = "https://www.thetvdb.com/" },
            new ThirdPartyTrackerEntity { Name = "MyAnimeList", ShortName = "MAL", Url = "https://myanimelist.net/" },
            new ThirdPartyTrackerEntity { Name = "Anime-Planet", ShortName = "AP", Url = "https://www.anime-planet.com/" },
            new ThirdPartyTrackerEntity { Name = "AniList", Url = "https://anilist.co/" },
            new ThirdPartyTrackerEntity { Name = "Anime News Network", ShortName = "ANN", Url = "https://www.animenewsnetwork.com/" },
            new ThirdPartyTrackerEntity { Name = "AllCinema", Url = "https://www.allcinema.net/" }
        };
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<ThirdPartyTrackerEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.Url).IsValidUrl();
    }
}
