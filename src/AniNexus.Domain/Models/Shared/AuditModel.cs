using EntityFrameworkCore.Triggered;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models an audit entry.
/// </summary>
public class AuditModel : IEntityTypeConfiguration<AuditModel>
{
    /// <summary>
    /// The table that was updated.
    /// </summary>
    public string Table { get; set; } = default!;

    /// <summary>
    /// The action taken upon one or more rows in that table.
    /// </summary>
    /// <remarks>
    /// See <see cref="ChangeType"/> for possible values.
    /// </remarks>
    public string Action { get; set; } = default!;

    /// <summary>
    /// The Id of the user who caused the change.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// The keys (and values where possible) of the rows that were changed.
    /// </summary>
    public string? AffectedKeys { get; set; }

    /// <summary>
    /// The date this change occurred.
    /// </summary>
    public DateTime Date { get; set; }

    public void Configure(EntityTypeBuilder<AuditModel> builder)
    {
        builder.ToTable("Audit");

        builder.HasNoKey();

        builder.HasIndex(m => m.Action);
        builder.HasIndex(m => m.Date).IsClustered();
        builder.HasIndex(m => m.Table);
        builder.HasIndex(m => m.UserId).HasFilter("[UserId] IS NOT NULL");

        builder.HasOne<ApplicationUserModel>().WithMany().HasForeignKey(m => m.UserId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);

        builder.Property(m => m.Table).HasComment("The table that was affected.");
        builder.Property(m => m.Action).HasComment("The action that was performed on the table.");
        builder.Property(m => m.AffectedKeys).HasComment("The values of the primary keys that were affected.");
        builder.Property(m => m.Date).HasComment("The date that this entry was added.");
    }
}
