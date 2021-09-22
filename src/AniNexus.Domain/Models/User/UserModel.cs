using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a AniNexus user.
/// </summary>
public class UserModel : IHasGuidPK, IHasRowVersion, IHasSoftDelete, IEntityTypeConfiguration<UserModel>, IValidatableObject
{
    /// <summary>
    /// The user's Id.
    /// </summary>
    public Guid Id { get; set; }

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

    #region Interface Properties
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
    /// The claims the user has.
    /// </summary>
    public IList<UserClaimMapModel> Claims { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder.ToTable("User");

        builder.HasIndex(m => m.Username);

        builder.Property(m => m.Username).HasComment("The user's username.").HasMaxLength(16);
        builder.Property(m => m.Email).HasComment("The user's email address.").HasMaxLength(250);
        builder.Property(m => m.EmailValidated).HasComment("Whether the user has validated their email address.");
        builder.Property(m => m.PasswordHash).HasComment("The user's hashed password.");
        builder.Property(m => m.TwoFactorEnabled).HasComment("Whether MFA is enabled for this user.");
        builder.Property(m => m.TwoFactorKey).HasComment("The MFA secret key for this user.");
        builder.Property(m => m.LockoutEnd).HasComment("The UTC time until which the user is locked out of their account.");
        builder.Property(m => m.AccessFailedCount).HasComment("The number of times the user entered incorrect credentials since their last login.");
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AccessFailedCount < 0)
        {
            yield return new ValidationResult("Access failure count must be greater than or equal to 0.", new[] { nameof(AccessFailedCount) });
        }
    }
}
