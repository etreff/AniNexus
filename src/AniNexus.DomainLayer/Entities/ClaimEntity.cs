namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a AniNexus user claim.
/// </summary>
public sealed class ClaimEntity : Entity<ClaimEntity>
{
    /// <summary>
    /// The type of the claim.
    /// </summary>
    public string Type { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<ClaimEntity> builder)
    {
        // 1. Index specification
        // 2. Navigation properties
        // 3. Propery specification
        builder.Property(m => m.Type).HasComment("The claim type.");
        // 4. Other
    }
}
