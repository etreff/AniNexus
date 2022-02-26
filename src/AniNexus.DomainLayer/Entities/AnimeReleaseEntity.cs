using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;
using AniNexus.Models;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a release of an anime in a specific locale.
/// </summary>
public sealed class AnimeReleaseEntity : AuditableEntity<AnimeReleaseEntity>, IHasRowVersion, IHasSoftDelete
{
    /// <summary>
    /// The Id of the anime this release belongs to.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The Id of the status that describes the production state of this release.
    /// </summary>
    /// <seealso cref="EMediaStatus"/>
    /// <seealso cref="MediaStatusTypeEntity"/>
    public byte StatusId { get; set; }

    /// <summary>
    /// The Id of the language of this release.
    /// </summary>
    public Guid LanguageId { get; set; }

    /// <summary>
    /// The Id of the age rating of this release.
    /// </summary>
    /// <remarks>
    /// Since anime releases tend to be distributed by language instead of
    /// region. The best we can do is apply the age rating for the general
    /// region that the language is associated with.
    /// </remarks>
    /// <seealso cref="EAnimeAgeRating"/>
    /// <seealso cref="AnimeAgeRatingTypeEntity"/>
    public byte AgeRatingId { get; set; }

    /// <summary>
    /// Whether this release is the primary release of the anime.
    /// </summary>
    /// <remarks>
    /// Only one release may be marked as primary.
    /// </remarks>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// The date on which the first episode of this anime released.
    /// </summary>
    public Date? StartDate { get; set; }

    /// <summary>
    /// The date on which the last episode of this anime released.
    /// </summary>
    public Date? EndDate { get; set; }

    /// <summary>
    /// The number of episodes this anime has or is expected to have
    /// in this release.
    /// </summary>
    public short? EpisodeCount { get; set; }

    /// <summary>
    /// The number of episodes that have actually been released.
    /// </summary>
    public short LatestEpisodeCount { get; set; }

    /// <summary>
    /// The day of the week (0 - 6) that this anime airs on.
    /// </summary>
    /// <seealso cref="DayOfWeek"/>
    public int? AirsOnDay { get; set; }

    /// <summary>
    /// The UTC time this anime normally airs at.
    /// </summary>
    public TimeOnly? AirTime { get; set; }

    /// <summary>
    /// A synopsis or description of the anime.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <summary>
    /// The URL to the release-specific website.
    /// </summary>
    /// <remarks>
    /// This should generally be left <see langword="null"/> unless a specific official
    /// website has been created for a different locale.
    /// </remarks>
    public string? WebsiteUrl { get; set; }

    /// <summary>
    /// The row version.
    /// </summary>
    public byte[] RowVersion { get; set; } = default!;

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }

    /// <summary>
    /// The anime this release belongs to.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The status that describes the production state of this release.
    /// </summary>
    public MediaStatusTypeEntity Status { get; set; } = default!;

    /// <summary>
    /// The language of this release.
    /// </summary>
    public LanguageEntity Language { get; set; } = default!;

    /// <summary>
    /// The age rating of this anime.
    /// </summary>
    public AnimeAgeRatingTypeEntity? AgeRating { get; set; }

    /// <summary>
    /// The primary name of the release.
    /// </summary>
    public NameEntity Name
    {
        get
        {
            if (Names is null || Names.Count == 0)
            {
                ThrowHelper.ThrowInvalidOperationException($"{nameof(Name)} has not been loaded.");
            }

            return Names.Single(static r => r.IsPrimary);
        }
    }

    /// <summary>
    /// The names of the anime in the region this release targets.
    /// </summary>
    public IList<NameEntity> Names { get; set; } = default!;

    /// <summary>
    /// The episodes of this release.
    /// </summary>
    public IList<AnimeEpisodeEntity> Episodes { get; set; } = default!;

    /// <summary>
    /// Trailers for this release.
    /// </summary>
    public IList<TrailerEntity> Trailers { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<AnimeReleaseEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => m.AnimeId);
        builder.HasIndex(m => m.StatusId);
        builder.HasIndex(m => m.LanguageId);
        builder.HasIndex(m => m.AgeRatingId).HasNotNullFilter();
        builder.HasIndex(m => m.StartDate).HasNotNullFilter();
        builder.HasIndex(m => m.EndDate).HasNotNullFilter();
        // 2. Navigation properties
        builder.HasOne(m => m.Anime).WithMany(m => m.Releases).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Status).WithMany().HasForeignKey(m => m.StatusId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Language).WithMany().HasForeignKey(m => m.LanguageId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.AgeRating).WithMany().HasForeignKey(m => m.AgeRatingId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        builder.OwnsMany(m => m.Names, name => name.ConfigureNameEntity());
        builder.OwnsMany(m => m.Trailers, trailer => trailer.ConfigureTrailerEntity());
        // 3. Propery specification
        builder.Property(m => m.IsPrimary).HasComment("Whether this is the primary release information for the anime.");
        builder.Property(m => m.StartDate).HasComment("The air date of the first episode in this locale.");
        builder.Property(m => m.EndDate).HasComment("The air date of the last episode in this locale.");
        builder.Property(m => m.EpisodeCount).HasComment("The expected number of entries in this release.");
        builder.Property(m => m.LatestEpisodeCount).HasComment("The actual number of entries in this release.");
        builder.Property(m => m.AirsOnDay).HasComment("The day of the week this entry airs on. Only relevant for anime with a regular release.");
        builder.Property(m => m.AirTime).HasComment("The UTC time this anime normally airs at.");
        builder.Property(m => m.Synopsis).HasComment("A synopsis or description of the anime.").HasMaxLength(2500);
        builder.Property(m => m.WebsiteUrl).HasComment("The URL to a place where the release can be purchased.");
        // 4. Other
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<AnimeReleaseEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.StartDate).IsLessThanOrEqualTo(m => m.EndDate ?? m.StartDate!.Value, "Start date must be before end date.");
        validator.Property(m => m.LatestEpisodeCount).IsLessThanOrEqualTo(m => m.EpisodeCount ?? short.MaxValue);
        validator.Property(m => m.AirsOnDay).IsBetween(0, 6);
        validator.Property(m => m.WebsiteUrl).IsValidUrl();
    }
}
