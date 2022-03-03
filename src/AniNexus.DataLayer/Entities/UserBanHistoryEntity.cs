namespace AniNexus.Data.Entities;

/// <summary>
/// Models a reason for why an AniNexus user was banned.
/// </summary>
public sealed class UserBanHistoryEntity : AuditableEntity<UserBanHistoryEntity>
{
    /// <summary>
    /// The Id of the user that got banned.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The UTC time at which the user was banned.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// The UTC time at which the ban ends. A <see langword="null"/> value indicates
    /// the ban does not end.
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// The reason the user was banned.
    /// </summary>
    public string Reason { get; set; } = default!;

    /// <summary>
    /// The user that was banned.
    /// </summary>
    public UserEntity User { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<UserBanHistoryEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => m.UserId);
        // 2. Navigation properties
        builder.HasOne(m => m.User).WithMany(m => m.BanHistory).HasForeignKey(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        builder.Property(m => m.StartTime).HasComment("The UTC time at which the user was banned.");
        builder.Property(m => m.EndTime).HasComment("The UTC time at which the ban ends.");
        builder.Property(m => m.Reason).HasComment("The reason the user was banned.").HasMaxLength(250);
        // 4. Other
    }
}
