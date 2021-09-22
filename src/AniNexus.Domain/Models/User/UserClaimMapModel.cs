using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between an AniNexus user and their claims.
/// </summary>
public class UserClaimMapModel : IEntityTypeConfiguration<UserClaimMapModel>
{
    /// <summary>
    /// The Id of the claim.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The Id of the claim.
    /// </summary>
    public int ClaimId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The user the claim belongs to.
    /// </summary>
    public UserModel User { get; set; } = default!;

    /// <summary>
    /// The claim.
    /// </summary>
    public UserClaimModel Claim { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<UserClaimMapModel> builder)
    {
        builder.ToTable("UserClaimMap");

        builder.HasKey(m => new { m.UserId, m.ClaimId });
        builder.HasIndex(m => m.ClaimId);

        builder.HasOne(m => m.User).WithMany(m => m.Claims).HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Claim).WithMany().HasForeignKey(m => m.ClaimId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.User).AutoInclude();
        builder.Navigation(m => m.Claim).AutoInclude();
    }
}
