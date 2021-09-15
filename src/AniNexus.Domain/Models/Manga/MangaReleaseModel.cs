using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using AniNexus.Domain.Validation;
using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a release of a manga in a specific region or locale.
/// </summary>
public class MangaReleaseModel : IHasAudit, IHasRowVersion, IHasSoftDelete, IEntityTypeConfiguration<MangaReleaseModel>, IValidatableObject
{
    /// <summary>
    /// The Id of the release.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The Id of the manga this release belongs to.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

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
    /// <seealso cref="EMangaAgeRating"/>
    /// <seealso cref="MangaAgeRatingTypeModel"/>
    public int? AgeRatingId { get; set; }

    /// <summary>
    /// Whether this release is the primary release of the manga.
    /// </summary>
    /// <remarks>
    /// Only one release may be marked as primary.
    /// </remarks>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// The date on which the first chapter of this manga released.
    /// </summary>
    public Date? StartDate { get; set; }

    /// <summary>
    /// The date on which the last chapter of this manga released.
    /// </summary>
    public Date? EndDate { get; set; }

    /// <summary>
    /// The number of volumes this manga has or is expected to have
    /// in this release.
    /// </summary>
    public short? VolumeCount { get; set; }

    /// <summary>
    /// The number of chapters this manga has or is expected to have
    /// in this release.
    /// </summary>
    public short? ChapterCount { get; set; }

    /// <summary>
    /// The number of volumes that have actually been released.
    /// </summary>
    public short LatestVolumeCount { get; set; }

    /// <summary>
    /// The number of chapters that have actually been released.
    /// </summary>
    public short LatestChapterCount { get; set; }

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
    /// The manga this release belongs to.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The status that describes the production state of this release.
    /// </summary>
    public MediaStatusTypeModel Status { get; set; } = default!;

    /// <summary>
    /// The locale of this release.
    /// </summary>
    public LocaleModel Locale { get; set; } = default!;

    /// <summary>
    /// The age rating of this manga.
    /// </summary>
    public MangaAgeRatingTypeModel? AgeRating { get; set; } = default!;

    /// <summary>
    /// The names of the manga in the region this release targets.
    /// </summary>
    public IList<NameModel> Names { get; set; } = default!;

    /// <summary>
    /// Models a name of a manga release.
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
    /// The volumes of this release.
    /// </summary>
    public IList<MangaVolumeModel> Volumes { get; set; } = default!;

    /// <summary>
    /// The chapters of this release.
    /// </summary>
    /// <remarks>
    /// Certain logistics may make it unfeasible to expect all chapters to
    /// be assigned to a volume. We increase our domain complexity by tracking
    /// chapters in both the release and the volume, but we will have improved
    /// correctness of information.
    /// </remarks>
    public IList<MangaChapterModel> Chapters { get; set; } = default!;
    #endregion

    #region Helper Properties
    /// <summary>
    /// Whether this release is considered NSFW in any region.
    /// </summary>
    /// <remarks>
    /// Since gore is considered NSFW in America, this value will also return <see langword="true"/>
    /// if <see cref="IsGore"/> would return <see langword="true"/>.
    /// </remarks>
    public bool IsNSFW => Manga.Tags?.Select(static m => m.Tag).Any(static t => t.IsNSFW || t.IsGore) ?? false;

    /// <summary>
    /// Whether this release has what is considered to be gore in any region.
    /// </summary>
    public bool IsGore => Manga.Tags?.Select(static m => m.Tag).Any(static t => t.IsGore) ?? false;

    /// <summary>
    /// The companies that have a license to stream or distribute this release.
    /// </summary>
    public IEnumerable<MediaCompanyMangaMapModel> Licensees
        => GetManga().Licensees.Where(l => l.ReleaseId == Id);
    #endregion

    public void Configure(EntityTypeBuilder<MangaReleaseModel> builder)
    {
        builder.ToTable("MangaRelease");

        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.MangaId);
        builder.HasIndex(m => m.StatusId);
        builder.HasIndex(m => m.AgeRatingId);
        builder.HasIndex(m => m.StartDate).HasFilter("[StartDate] IS NOT NULL");
        builder.HasIndex(m => m.EndDate).HasFilter("[EndDate] IS NOT NULL");

        builder.HasOne(m => m.Manga).WithMany(m => m.Releases).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Status).WithMany().HasForeignKey(m => m.StatusId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Locale).WithMany().HasForeignKey(m => m.LocaleId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.AgeRating).WithMany().HasForeignKey(m => m.AgeRatingId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);

        builder.OwnsMany(m => m.Names, name =>
        {
            name.ToTable("MangaReleaseName");

            name.Property(m => m.NativeName).HasComment("The native name.").HasColumnName(nameof(NameModel.NativeName));
            name.Property(m => m.RomajiName).HasComment("The romanization of the native name.").HasColumnName(nameof(NameModel.RomajiName));
            name.Property(m => m.EnglishName).HasComment("The name in English.").HasColumnName(nameof(NameModel.EnglishName));
            name.Property(m => m.IsPrimary).HasComment("Whether this name is the primary name of the release.").HasColumnName(nameof(NameModel.IsPrimary));
        });

        builder.Property(m => m.IsPrimary).HasComment("Whether this is the primary release information for the manga.");
        builder.Property(m => m.StartDate).HasComment("The air date of the first chapter in this locale.");
        builder.Property(m => m.EndDate).HasComment("The air date of the last chapter in this locale.");
        builder.Property(m => m.VolumeCount).HasComment("The expected number of volumes in this release.");
        builder.Property(m => m.ChapterCount).HasComment("The expected number of chapters in this release.");
        builder.Property(m => m.LatestVolumeCount).HasComment("The actual number of volumes in this release.").HasDefaultValue(0);
        builder.Property(m => m.LatestChapterCount).HasComment("The actual number of chapters in this release.").HasDefaultValue(0);
        builder.Property(m => m.WebsiteUrl).HasComment("The URL to a place where the release can be purchased.");
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate.HasValue && EndDate.HasValue && StartDate > EndDate)
        {
            yield return new ValidationResult("The start date must be after the end date.", new[] { nameof(StartDate), nameof(EndDate) });
        }

        if (VolumeCount.HasValue && LatestVolumeCount > VolumeCount)
        {
            yield return new ValidationResult("The latest volume count cannot exceed the actual volume count.", new[] { nameof(LatestVolumeCount) });
        }

        if (ChapterCount.HasValue && LatestChapterCount > ChapterCount)
        {
            yield return new ValidationResult("The latest chapter count cannot exceed the actual chapter count.", new[] { nameof(LatestChapterCount) });
        }

        if (!UriValidator.Validate(WebsiteUrl, nameof(WebsiteUrl), out var urlValidationResult))
        {
            yield return urlValidationResult;
        }
    }

    /// <summary>
    /// Returns the primary name of the release.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public NameModel GetPrimaryName()
    {
        return Names.Single(static r => r.IsPrimary);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private MangaModel GetManga()
    {
        if (Manga is null)
        {
            throw new InvalidOperationException("Manga must be loaded via Include() before this property can be used.");
        }

        return Manga;
    }
}
