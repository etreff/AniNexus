using System.ComponentModel.DataAnnotations;
using AniNexus.Data.Validation;

namespace AniNexus.Data.Entities;

/// <summary>
/// Models a franchise or intellectual property that all anime, manga, and games
/// belong to.
/// </summary>
public sealed class FranchiseEntity : AuditableEntity<FranchiseEntity>, IHasRowVersion, IHasSoftDelete, IHasImage
{
    /// <summary>
    /// The name of this franchise.
    /// </summary>
    public NameEntity Name { get; set; } = default!;

    /// <summary>
    /// A synopsis of the series.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <inheritdoc/>
    public Guid? ImageId { get; set; }

    /// <summary>
    /// The row version.
    /// </summary>
    public byte[] RowVersion { get; set; } = default!;

    /// <summary>
    /// Whether this entry is soft-deleted. It will not be included in queries unless
    /// <see cref="M:Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.IgnoreQueryFilters``1(System.Linq.IQueryable{``0})" />
    /// is invoked.
    /// </summary>
    public bool IsSoftDeleted { get; set; }

    /// <summary>
    /// The aliases of the series.
    /// </summary>
    public IList<NameEntity> Aliases { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<FranchiseEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        // 2. Navigation properties
        builder.OwnsOne(m => m.Name, static name => name.ConfigureOwnedEntity(false));
        builder.OwnsMany(m => m.Aliases, static name => name.ConfigureOwnedEntity());
        // 3. Propery specification
        builder.Property(m => m.Synopsis).HasComment("A synopsis of the media series.").HasMaxLength(2500);
        // 4. Other
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<FranchiseEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.ValidateOwnedEntity(m => m.Name);
        validator.ValidateOwnedEntities(m => m.Aliases!);
        validator.Property(m => m.Synopsis).HasLengthLessThanOrEqualTo(1250);
    }
}
