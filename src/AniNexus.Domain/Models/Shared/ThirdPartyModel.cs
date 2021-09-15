using AniNexus.Domain.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a third party tracker.
/// </summary>
public class ThirdPartyModel : IHasAudit, IHasSoftDelete, IEntityTypeConfiguration<ThirdPartyModel>, IValidatableObject
{
    /// <summary>
    /// The Id of the tracker.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the tracker.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// The short name or alias of the tracker.
    /// </summary>
    public string? ShortName { get; set; }

    /// <summary>
    /// The romanization of the name.
    /// </summary>
    public string? RomajiName { get; set; }

    /// <summary>
    /// A URL to the tracker's website.
    /// </summary>
    public string? Url { get; set; } = default!;

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

    public void Configure(EntityTypeBuilder<ThirdPartyModel> builder)
    {
        builder.ToTable("ThirdParty");

        builder.HasKey(m => m.Id);

        builder.HasData(
            new ThirdPartyModel { Id = 1, Name = "AniDB", Url = "https://anidb.net/" },
            new ThirdPartyModel { Id = 2, Name = "TheTVDB", Url = "https://www.thetvdb.com/" },
            new ThirdPartyModel { Id = 3, Name = "MyAnimeList", ShortName = "MAL", Url = "https://myanimelist.net/" },
            new ThirdPartyModel { Id = 4, Name = "Anime-Planet", ShortName = "AP", Url = "https://www.anime-planet.com/" },
            new ThirdPartyModel { Id = 5, Name = "AniList", Url = "https://anilist.co/" },
            new ThirdPartyModel { Id = 6, Name = "Anime News Network", ShortName = "ANN", Url = "https://www.animenewsnetwork.com/" },
            new ThirdPartyModel { Id = 7, Name = "AllCinema", Url = "https://www.allcinema.net/" });

        builder.Property(m => m.Name).HasComment("The name of the third party tracker.").HasMaxLength(64).UseCollation(Collation.Japanese);
        builder.Property(m => m.ShortName).HasComment("The short name or alias of the third party tracker.").HasMaxLength(16).UseCollation(Collation.Japanese);
        builder.Property(m => m.RomajiName).HasComment("The romanized name of the third party tracker.").HasMaxLength(100);
        builder.Property(m => m.Url).HasComment("The URL of the third party tracker's website homepage.").HasMaxLength(128);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!UriValidator.Validate(Url, nameof(Url), out var urlValidationResult))
        {
            yield return urlValidationResult;
        }
    }
}
