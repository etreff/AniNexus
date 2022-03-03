namespace AniNexus.Data.Entities;

/// <summary>
/// Models a claim that a AniNexus user has.
/// </summary>
public sealed class UserClaimEntity : Entity<UserClaimEntity>
{
    /// <summary>
    /// The Id of the user the claim belongs to.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The Id of the claim.
    /// </summary>
    public Guid ClaimId { get; set; }

    /// <summary>
    /// The value of the claim.
    /// </summary>
    public string ClaimValue { get; set; } = default!;

    /// <summary>
    /// The user the claim belongs to.
    /// </summary>
    public UserEntity User { get; set; } = default!;

    /// <summary>
    /// The claim.
    /// </summary>
    public ClaimEntity Claim { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<UserClaimEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.UserId, m.ClaimId }).IsUnique();
        builder.HasIndex(m => m.UserId);
        // 2. Navigation properties
        builder.HasOne(m => m.User).WithMany(m => m.Claims).HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Claim).WithMany().HasForeignKey(m => m.ClaimId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // Justification - we will almost always want the type of the claim as well.
        builder.Navigation(m => m.Claim).AutoInclude();
        // 3. Propery specification
        builder.Property(m => m.ClaimValue).HasComment("The claim value.");
        // 4. Other
    }
}
