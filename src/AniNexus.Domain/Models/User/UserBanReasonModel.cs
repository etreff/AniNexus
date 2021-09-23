using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a reason for why an AniNexus user was banned.
/// </summary>
public class UserBanReasonModel : IEntityTypeConfiguration<UserBanReasonModel>
{
    /// <summary>
    /// The Id of the record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The Id of the user that got banned.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The UTC time at which the user was banned.
    /// </summary>
    public DateTime BannedAt { get; set; }

    /// <summary>
    /// The UTC time until which the user was banned.
    /// </summary>
    public DateTime? BannedUntil { get; set; }

    /// <summary>
    /// The reason the user was banned.
    /// </summary>
    public string Reason { get; set; } = default!;

    #region Navigation Properties
    /// <summary>
    /// The user that was banned.
    /// </summary>
    public UserModel User { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<UserBanReasonModel> builder)
    {
        builder.ToTable("UserBanReason");

        builder.HasAlternateKey(m => m.Id);

        builder.HasOne(m => m.User).WithMany(m => m.BanReasons).HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.BannedAt).HasComment("The UTC time at which the user was banned.");
        builder.Property(m => m.BannedUntil).HasComment("The UTC time until which the user is banned.");
        builder.Property(m => m.Reason).HasComment("The reason the user was banned.").HasMaxLength(250);
    }
}
