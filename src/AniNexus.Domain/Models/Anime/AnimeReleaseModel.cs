using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using AniNexus.Domain.Validation;
using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a release of an anime in a specific region or locale.
/// </summary>
public class AnimeReleaseModel : IHasAudit, IHasRowVersion, IHasSoftDelete, IEntityTypeConfiguration<AnimeReleaseModel>, IValidatableObject
{
    /// <summary>
    /// The Id of the release.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The Id of the anime this release belongs to.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The Id of the status that describes the production state of this release.
    /// </summary>
    /// <seealso cref="EMediaStatus"/>
    /// <seealso cref="MediaStatusTypeModel"/>
    public int StatusId { get; set; }

    /// <summary>
    /// The Id of the locale of this release.
    /// </summary>
    /// <seealso cref="LocaleModel"/>
    public int LocaleId { get; set; }

    /// <summary>
    /// The Id of the age rating of this release.
    /// </summary>
    /// <seealso cref="EAnimeAgeRating"/>
    /// <seealso cref="AnimeAgeRatingTypeModel"/>
    public int? AgeRatingId { get; set; }

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
    public TimeSpan? AirTime { get; set; }

    /// <summary>
    /// The URL to the release-specific website.
    /// </summary>
    /// <remarks>
    /// This should generally be left <see langword="null"/> unless a specific official
    /// website has been created for a different locale.
    /// </remarks>
    public string? WebsiteUrl { get; set; }

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
    /// The row version.
    /// </summary>
    public byte[] RowVersion { get; set; } = default!;

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }
    #endregion

    #region Navigation Properties
    /// <summary>
    /// The anime this release belongs to.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The status that describes the production state of this release.
    /// </summary>
    public MediaStatusTypeModel Status { get; set; } = default!;

    /// <summary>
    /// The locale of this release.
    /// </summary>
    public LocaleModel Locale { get; set; } = default!;

    /// <summary>
    /// The age rating of this anime.
    /// </summary>
    public AnimeAgeRatingTypeModel? AgeRating { get; set; } = default!;

    /// <summary>
    /// The names of the anime in the region this release targets.
    /// </summary>
    public IList<NameModel> Names { get; set; } = default!;

    /// <summary>
    /// Models a name of an anime release.
    /// </summary>
    public class NameModel
    {
        /// <summary>
        /// The name in the native language.
        /// </summary>
        public string? NativeName { get; set; }

        /// <summary>
        /// The romanization of the native name.
        /// </summary>
        public string? RomajiName { get; set; }

        /// <summary>
        /// The name in English.
        /// </summary>
        public string? EnglishName { get; set; }

        /// <summary>
        /// Whether the name is the primary name.
        /// </summary>
        public bool IsPrimary { get; set; }
    }

    /// <summary>
    /// The episodes of this release.
    /// </summary>
    public IList<AnimeEpisodeModel> Episodes { get; set; } = default!;

    /// <summary>
    /// Trailers for this release.
    /// </summary>
    public IList<TrailerModel> Trailers { get; set; } = default!;

    /// <summary>
    /// Models an anime release trailer.
    /// </summary>
    public class TrailerModel : IValidatableObject
    {
        /// <summary>
        /// A link to the trailer.
        /// </summary>
        /// <remarks>
        /// This URL must be valid.
        /// </remarks>
        public string ResourceUrl { get; set; } = default!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!UriValidator.Validate(ResourceUrl, nameof(ResourceUrl), out var urlValidationResult))
            {
                yield return urlValidationResult;
            }
        }
    }

    /// <summary>
    /// The live-action actors who portray characters in this release.
    /// </summary>
    public IList<MediaCharacterActorMapModel> LiveActors { get; set; } = default!;

    /// <summary>
    /// The voice actors who portray characters in this release.
    /// </summary>
    public IList<MediaCharacterVoiceActorMapModel> VoiceActors { get; set; } = default!;
    #endregion

    #region Helper Properties
    /// <summary>
    /// The primary name of the release.
    /// </summary>
    public NameModel Name => Names.Single(static r => r.IsPrimary);

    /// <summary>
    /// Whether this release is considered NSFW in any region.
    /// </summary>
    /// <remarks>
    /// Since gore is considered NSFW in America, this value will also return <see langword="true"/>
    /// if <see cref="IsGore"/> would return <see langword="true"/>.
    /// </remarks>
    public bool IsNSFW => Anime.Tags?.Select(static m => m.Tag).Any(static t => t.IsNSFW || t.IsGore) ?? false;

    /// <summary>
    /// Whether this release has what is considered to be gore in any region.
    /// </summary>
    public bool IsGore => Anime.Tags?.Select(static m => m.Tag).Any(static t => t.IsGore) ?? false;

    /// <summary>
    /// The companies that have a license to stream or distribute this release.
    /// </summary>
    public IEnumerable<MediaCompanyAnimeMapModel> Licensees
        => GetAnime().Licensees.Where(l => l.ReleaseId == Id);
    #endregion

    public void Configure(EntityTypeBuilder<AnimeReleaseModel> builder)
    {
        builder.ToTable("AnimeRelease");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.AnimeId);
        builder.HasIndex(m => m.StatusId);
        builder.HasIndex(m => m.LocaleId);
        builder.HasIndex(m => m.AgeRatingId).HasFilter("[AgeRatingId] IS NOT NULL");
        builder.HasIndex(m => m.StartDate).HasFilter("[StartDate] IS NOT NULL");
        builder.HasIndex(m => m.EndDate).HasFilter("[EndDate] IS NOT NULL");

        builder.HasOne(m => m.Anime).WithMany(m => m.Releases).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Status).WithMany().HasForeignKey(m => m.StatusId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Locale).WithMany().HasForeignKey(m => m.LocaleId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.AgeRating).WithMany().HasForeignKey(m => m.AgeRatingId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);

        builder.OwnsMany(m => m.Names, name =>
        {
            name.ToTable("AnimeReleaseName");

            name.Property(m => m.NativeName).HasComment("The native name.").HasColumnName(nameof(NameModel.NativeName));
            name.Property(m => m.RomajiName).HasComment("The romanization of the native name.").HasColumnName(nameof(NameModel.RomajiName));
            name.Property(m => m.EnglishName).HasComment("The name in English.").HasColumnName(nameof(NameModel.EnglishName));
            name.Property(m => m.IsPrimary).HasComment("Whether this name is the primary name of the release.").HasColumnName(nameof(NameModel.IsPrimary));
        });

        builder.OwnsMany(m => m.Trailers, trailer =>
        {
            trailer.ToTable("AnimeReleaseTrailer");

            trailer.Property(m => m.ResourceUrl).HasComment("The URL of the trailer or promotional video.").HasColumnName(nameof(TrailerModel.ResourceUrl));
        });

        builder.Property(m => m.IsPrimary).HasComment("Whether this is the primary release information for the anime.");
        builder.Property(m => m.StartDate).HasComment("The air date of the first episode in this locale.");
        builder.Property(m => m.EndDate).HasComment("The air date of the last episode in this locale.");
        builder.Property(m => m.EpisodeCount).HasComment("The expected number of entries in this release.");
        builder.Property(m => m.LatestEpisodeCount).HasComment("The actual number of entries in this release.").HasDefaultValue(0);
        builder.Property(m => m.AirsOnDay).HasComment("The day of the week this entry airs on. Only relevant for anime with a regular release.");
        builder.Property(m => m.AirTime).HasComment("The UTC time this anime normally airs at.");
        builder.Property(m => m.WebsiteUrl).HasComment("The URL to a place where the release can be purchased.");
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate.HasValue && EndDate.HasValue && StartDate > EndDate)
        {
            yield return new ValidationResult("The start date must be after the end date.", new[] { nameof(StartDate), nameof(EndDate) });
        }

        if (EpisodeCount.HasValue && LatestEpisodeCount > EpisodeCount)
        {
            yield return new ValidationResult("The latest episode count cannot exceed the actual episode count.", new[] { nameof(LatestEpisodeCount) });
        }

        if (AirsOnDay.HasValue && !AirsOnDay.Value.IsBetween(0, 6))
        {
            yield return new ValidationResult("The day must be between 0 and 6 inclusive.", new[] { nameof(AirsOnDay) });
        }

        if (AirTime > new TimeSpan(23, 59, 59))
        {
            yield return new ValidationResult("The air time may not be greater than 23:59:59", new[] { nameof(AirTime) });
        }

        if (!UriValidator.Validate(WebsiteUrl, nameof(WebsiteUrl), out var urlValidationResult))
        {
            yield return urlValidationResult;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private AnimeModel GetAnime()
    {
        if (Anime is null)
        {
            throw new InvalidOperationException("Anime must be loaded via Include() before this property can be used.");
        }

        return Anime;
    }
}
