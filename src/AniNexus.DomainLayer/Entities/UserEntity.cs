using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a AniNexus user.
/// </summary>
public sealed class UserEntity : Entity<UserEntity>, IHasRowVersion, IHasSoftDelete
{
    /// <summary>
    /// The user's username.
    /// </summary>
    public string Username { get; set; } = default!;

    /// <summary>
    /// The user's email address.
    /// </summary>
    public string Email { get; set; } = default!;

    /// <summary>
    /// Whether the user has confirmed their email address.
    /// </summary>
    public bool EmailValidated { get; set; }

    /// <summary>
    /// A hash of the user's password.
    /// </summary>
    public string PasswordHash { get; set; } = default!;

    /// <summary>
    /// Whether the account has 2FA/MFA enabled.
    /// </summary>
    public bool TwoFactorEnabled { get; set; }

    /// <summary>
    /// The key to use for MFA validation.
    /// </summary>
    public string? TwoFactorKey { get; set; }

    /// <summary>
    /// The UTC time until which the user is locked out of their account.
    /// </summary>
    public DateTime? LockoutEnd { get; set; }

    /// <summary>
    /// The number of times the user entered credentials for the account that were incorrect
    /// since their last login.
    /// </summary>
    public int AccessFailedCount { get; set; }

    /// <summary>
    /// Whether the user is banned, restricting access to certain site features.
    /// </summary>
    public bool IsBanned { get; set; }

    /// <summary>
    /// The UTC time until which the user is banned. Setting this to <see langword="null"/> will
    /// permenantly ban the user.
    /// </summary>
    public DateTime? BannedUntil { get; set; }

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
    /// The claims the user has.
    /// </summary>
    public IList<UserClaimEntity> Claims { get; set; } = default!;

    /// <summary>
    /// Reasons why a user was banned.
    /// </summary>
    public IList<UserBanHistoryEntity> BanHistory { get; set; } = default!;

    /// <summary>
    /// User tags that the user has submitted and were approved.
    /// </summary>
    public IList<UserTagEntity> ApprovedTags { get; set; } = default!;

    /// <summary>
    /// User tags that the user has submitted and are pending.
    /// </summary>
    public IList<UserTagPendingEntity> PendingTags { get; set; } = default!;

    /// <summary>
    /// User tags that the user has submitted and were rejected.
    /// </summary>
    public IList<UserTagRejectedEntity> RejectedTags { get; set; } = default!;

    /// <summary>
    /// User tag translations that the user has submitted.
    /// </summary>
    public IList<UserTagTranslationEntity> ApprovedTagTranslations { get; set; } = default!;

    /// <summary>
    /// User tag translations that the user has submitted and are still pending approval.
    /// </summary>
    public IList<UserTagPendingTranslationEntity> PendingTagTranslations { get; set; } = default!;

    /// <summary>
    /// User tag translations that the user has submitted and were rejected.
    /// </summary>
    public IList<UserTagRejectedTranslationEntity> RejectedTagTranslations { get; set; } = default!;

    /// <summary>
    /// Tags that this user has applied to anime that are pending enough votes to be applied.
    /// </summary>
    public IList<AnimeTagMapPendingEntity> PendingAnimeTags { get; set; } = default!;

    /// <summary>
    /// Anime reviews the user has written.
    /// </summary>
    public IList<AnimeUserReviewEntity> AnimeReviews { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<UserEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => m.Username);
        // 2. Navigation properties
        // 3. Propery specification
        builder.Property(m => m.Username).HasComment("The user's username.").HasMaxLength(16);
        builder.Property(m => m.Email).HasComment("The user's email address.").HasMaxLength(250);
        builder.Property(m => m.EmailValidated).HasComment("Whether the user has validated their email address.");
        builder.Property(m => m.PasswordHash).HasComment("The user's hashed password.");
        builder.Property(m => m.TwoFactorEnabled).HasComment("Whether MFA is enabled for this user.");
        builder.Property(m => m.TwoFactorKey).HasComment("The MFA secret key for this user.");
        builder.Property(m => m.LockoutEnd).HasComment("The UTC time until which the user is locked out of their account.");
        builder.Property(m => m.AccessFailedCount).HasComment("The number of times the user entered incorrect credentials since their last login.");
        builder.Property(m => m.IsBanned).HasComment("Whether the user is banned.");
        builder.Property(m => m.BannedUntil).HasComment("The UTC time until which the user is banned. A null value will permanently ban the user.");
        // 4. Other
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<UserEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.AccessFailedCount).IsGreaterThanOrEqualTo(0);
    }
}
