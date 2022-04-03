using System.ComponentModel.DataAnnotations;
using AniNexus.Data.Validation;

namespace AniNexus.Data.Entities;

/// <summary>
/// An installment in a release/series.
/// </summary>
/// <typeparam name="TInstallmentEntity">The installment entity type.</typeparam>
/// <typeparam name="TReleaseEntity">The release entity type that this installment is associated with.</typeparam>
public abstract class InstallmentEntity<TInstallmentEntity, TReleaseEntity> : AuditableEntity<TInstallmentEntity>, IHasRowVersion
    where TInstallmentEntity : InstallmentEntity<TInstallmentEntity, TReleaseEntity>
    where TReleaseEntity : ReleaseEntity<TReleaseEntity, TInstallmentEntity>
{
    /// <summary>
    /// The Id of the release entity associated with this installment.
    /// </summary>
    public Guid ReleaseId { get; set; }

    /// <summary>
    /// The installment number (episode or chapter number).
    /// </summary>
    public short Number { get; set; }

    /// <summary>
    /// The name of the episode.
    /// </summary>
    public NameEntity? Name { get; set; }

    /// <summary>
    /// The date the installment was released.
    /// </summary>
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// A synopsis of the installment.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <summary>
    /// A URL to a place where the user can legally watch the episode.
    /// </summary>
    public string? PurchaseUrl { get; set; }

    /// <summary>
    /// The row version.
    /// </summary>
    public byte[] RowVersion { get; set; } = default!;

    /// <summary>
    /// The release entity associated with this installment.
    /// </summary>
    public TReleaseEntity Release { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<TInstallmentEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => m.ReleaseId);
        builder.HasIndex(m => m.ReleaseDate).HasNotNullFilter();
        // 2. Navigation properties
        builder.HasOne(m => m.Release).WithMany(m => m.Installments).HasForeignKey(m => m.ReleaseId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.OwnsOne(m => m.Name, static owned => owned.ConfigureOwnedEntity());
        // 3. Propery specification
        builder.Property(m => m.Number).HasComment("The installment number (episode or chapter number).");
        builder.Property(m => m.PurchaseUrl).HasComment("A URL to a place where the user can legally purchase the installment.");
        builder.Property(m => m.ReleaseDate).HasComment("The date the installment was released.");
        builder.Property(m => m.Synopsis).HasMaxLength(2500).HasComment("The synopsis of the installment.");
        // 4. Other
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<TInstallmentEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.ReleaseId).IsNotEmpty();
        validator.Property(m => m.PurchaseUrl).IsValidUrl();
        validator.Property(m => m.Number).IsGreaterThanOrEqualTo((short)1);
        validator.Property(m => m.Synopsis).HasLengthLessThanOrEqualTo(1250);

        validator.ValidateOwnedEntity(m => m.Name);
    }
}
