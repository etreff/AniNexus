namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a code that validates a user's email address.
/// </summary>
public sealed class UserEmailCodeEntity : Entity<UserEmailCodeEntity>
{
    /// <summary>
    /// The code to validate the email address.
    /// </summary>
    public string Code { get; set; } = default!;

    /// <summary>
    /// The Id of the user this code is for.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// The UTC time until which the code is valid.
    /// </summary>
    public DateTime ValidUntil { get; set; }

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<UserEmailCodeEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => m.Code);
        builder.HasIndex(m => m.UserId);
        // 2. Navigation properties
        builder.HasOne<UserEntity>().WithOne().HasForeignKey<UserEmailCodeEntity>(m => m.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 3. Propery specification
        builder.Property(m => m.Code).HasComment("The code to validate the email address.");
        builder.Property(m => m.ValidUntil).HasComment("The UTC time until which the code is valid.");
        // 4. Other
    }
}
