using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a code that validates a user's email address.
/// </summary>
public class UserEmailCodeModel : IHasGuidPK, IEntityTypeConfiguration<UserEmailCodeModel>
{
    /// <summary>
    /// The user's Id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The code to validate the email address.
    /// </summary>
    public string Code { get; set; } = default!;

    /// <summary>
    /// The UTC time until which the code is valid.
    /// </summary>
    public DateTime ValidUntil { get; set; }

    public void Configure(EntityTypeBuilder<UserEmailCodeModel> builder)
    {
        builder.ToTable("UserEmailCode");

        builder.HasOne<UserModel>().WithOne().HasForeignKey<UserEmailCodeModel>(m => m.Id).IsRequired().OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.Code).HasComment("The code to validate the email address.");
        builder.Property(m => m.ValidUntil).HasComment("The UTC time until which the code is valid.");
    }
}
