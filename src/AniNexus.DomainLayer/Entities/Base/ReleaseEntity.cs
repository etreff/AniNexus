using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;
using AniNexus.Models;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a media release.
/// </summary>
public abstract class ReleaseEntity<TReleaseEntity, TInstallmentEntity> : AuditableEntity<TReleaseEntity>, IHasRowVersion, IHasSoftDelete
    where TReleaseEntity : ReleaseEntity<TReleaseEntity, TInstallmentEntity>
    where TInstallmentEntity : InstallmentEntity<TInstallmentEntity, TReleaseEntity>
{
    /// <summary>
    /// The Id of the owner.
    /// </summary>
    public Guid OwnerId { get; set; } = default!;

    /// <summary>
    /// The name of the release.
    /// </summary>
    public NameEntity Name { get; set; } = default!;

    /// <summary>
    /// Alternate names for this release.
    /// </summary>
    public IList<string> Aliases { get; set; } = default!;

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
    /// Releases tend to be distributed by language instead of region. The best we can do
    /// is apply the age rating for the general region that the language is associated with.
    /// </remarks>
    public byte AgeRatingId { get; set; }

    /// <summary>
    /// Whether this release is the primary release.
    /// </summary>
    /// <remarks>
    /// Only one release may be marked as primary.
    /// </remarks>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// The date on which the first episode/chapter/game was released.
    /// </summary>
    public Date? StartDate { get; set; }

    /// <summary>
    /// The date on which the last episode/chapter/game was released. In the case of
    /// games this will be when the product received it last update or was no longer
    /// supported by its developers (server shutdown for example).
    /// </summary>
    public Date? EndDate { get; set; }

    /// <summary>
    /// The number of episodes or chapters that have actually been released.
    /// </summary>
    public short LatestElementCount { get; set; }

    /// <summary>
    /// The number of episodes or chapters this release has or is expected to have.
    /// </summary>
    public short? ElementCount { get; set; }

    /// <summary>
    /// The day of the week (0 - 6) that this media releases on.
    /// </summary>
    /// <seealso cref="DayOfWeek"/>
    public int? ReleasedOnDay { get; set; }

    /// <summary>
    /// The UTC time this media normally releases at.
    /// </summary>
    public TimeOnly? ReleaseTime { get; set; }

    /// <summary>
    /// A synopsis or description of the release.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <summary>
    /// The URL to the website that sells the release.
    /// </summary>
    public string? PurchaseUrl { get; set; }

    /// <summary>
    /// The URL to the release-specific website.
    /// </summary>
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
    /// The status that describes the production state of this release.
    /// </summary>
    public MediaStatusTypeEntity Status { get; set; } = default!;

    /// <summary>
    /// The language of this release.
    /// </summary>
    public LanguageEntity Language { get; set; } = default!;

    /// <summary>
    /// Trailers for this release.
    /// </summary>
    public IList<TrailerEntity> Trailers { get; set; } = default!;

    /// <summary>
    /// The installments of this release.
    /// </summary>
    public IList<TInstallmentEntity> Installments { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<TReleaseEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => m.OwnerId);
        builder.HasIndex(m => m.StatusId);
        builder.HasIndex(m => m.LanguageId);
        builder.HasIndex(m => m.AgeRatingId);
        builder.HasIndex(m => m.StartDate).HasNotNullFilter();
        builder.HasIndex(m => m.EndDate).HasNotNullFilter();
        // 2. Navigation properties
        builder.HasOne(m => m.Status).WithMany().HasForeignKey(m => m.StatusId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(m => m.Language).WithMany().HasForeignKey(m => m.LanguageId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.OwnsMany(m => m.Trailers, static owned => owned.ConfigureOwnedEntity());
        builder.OwnsOne(m => m.Name, static owned => owned.ConfigureOwnedEntity(false));
        // 3. Propery specification
        builder.Property(m => m.Aliases).HasColumnType("nvarchar(500)").IsRequired(false).HasListConversion();
        builder.Property(m => m.ElementCount).HasComment("The number of episodes or chapters this release has or is expected to have.");
        builder.Property(m => m.EndDate).HasComment("The air date of the last episode in this locale.");
        builder.Property(m => m.IsPrimary).HasComment("Whether this is the primary release information for the anime.");
        builder.Property(m => m.LatestElementCount).HasComment("The number of episodes or chapters that have actually been released.");
        builder.Property(m => m.PurchaseUrl).HasComment("The URL to the website that sells the release.");
        builder.Property(m => m.ReleasedOnDay).HasComment("The day of the week (0 - 6) that this media releases on.");
        builder.Property(m => m.ReleaseTime).HasComment("The UTC time this media normally releases at.");
        builder.Property(m => m.StartDate).HasComment("The air date of the first episode in this locale.");
        builder.Property(m => m.Synopsis).HasComment("A synopsis or description of the release.").HasMaxLength(2500);
        builder.Property(m => m.WebsiteUrl).HasComment("The URL to the release-specific website.");
        // 4. Other
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<TReleaseEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.ValidateOwnedEntity(m => m.Name);
        validator.Property(m => m.Aliases)!.ForEach((v, s) => { if (string.IsNullOrWhiteSpace(s)) { v.AddValidationResult(new ValidationResult("Value cannot be null, empty, or whitespace.")); } });
        validator.Property(m => m.ElementCount).IsGreaterThanOrEqualTo((short)0);
        validator.Property(m => m.LatestElementCount).IsLessThanOrEqualTo(m => m.ElementCount ?? short.MaxValue);
        validator.Property(m => m.PurchaseUrl).IsValidUrl();
        validator.Property(m => m.ReleasedOnDay).IsBetween(0, 6);
        validator.AddValidationRule((c, e) =>
        {
            if (e.StartDate.HasValue && e.EndDate.HasValue && e.StartDate.Value > e.EndDate.Value)
            {
                c.AddValidationResult(new ValidationResult("Start date must be less than or equal to end date.", new[] { nameof(StartDate), nameof(EndDate) }));
            }
        });
        validator.Property(m => m.Synopsis).HasLengthLessThanOrEqualTo(1500);
        validator.Property(m => m.WebsiteUrl).IsValidUrl();
    }
}
