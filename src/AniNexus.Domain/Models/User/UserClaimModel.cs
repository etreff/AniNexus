using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a AniNexus user claim.
/// </summary>
public class UserClaimModel : IEntityTypeConfiguration<UserClaimModel>
{
    /// <summary>
    /// The Id of the claim.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The type of the claim.
    /// </summary>
    public string ClaimType { get; set; } = default!;

    /// <summary>
    /// The value of the claim.
    /// </summary>
    public string ClaimValue { get; set; } = default!;

    public void Configure(EntityTypeBuilder<UserClaimModel> builder)
    {
        builder.ToTable("UserClaim");

        builder.Property(m => m.ClaimType).HasComment("The claim type.");
        builder.Property(m => m.ClaimValue).HasComment("The claim value.");
    }
}
