using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a company. This includes producers, studios, circles, groups, and publishers.
/// </summary>
/// <remarks>
/// This does not include artists, bands, and the like.
/// </remarks>
public class CompanyEntity : AuditableEntity<CompanyEntity>, IHasRowVersion, IHasSoftDelete, IHasPublicId
{
    /// <summary>
    /// The public Id of the company.
    /// </summary>
    public int PublicId { get; set; }

    /// <summary>
    /// The Id of the company type.
    /// </summary>
    public byte CompanyTypeId { get; set; }

    /// <summary>
    /// The name of this company.
    /// </summary>
    public NameEntity Name { get; set; } = default!;

    /// <summary>
    /// The date this company was established or founded.
    /// </summary>
    public DateOnly? CreationDate { get; set; }

    /// <summary>
    /// The city this company was established or founded.
    /// </summary>
    public string? CreationCity { get; set; }

    /// <summary>
    /// The state this company was established or founded.
    /// </summary>
    public string? CreationState { get; set; }

    /// <summary>
    /// The country this company was established or founded.
    /// </summary>
    public string? CreationCountry { get; set; }

    /// <summary>
    /// A description of this company.
    /// </summary>
    public string? Description { get; set; }

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
    /// The company type.
    /// </summary>
    public CompanyTypeEntity CompanyType { get; set; } = default!;

    /// <summary>
    /// Anime that this company had a role in creating.
    /// </summary>
    public IList<CompanyAnimeMapEntity> AnimeRoles { get; set; } = default!;

    /// <summary>
    /// Albums that this company has published.
    /// </summary>
    public IList<AlbumEntity> Albums { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<CompanyEntity> builder)
    {
        base.ConfigureEntity(builder);

        // 1. Index specification
        builder.HasIndex(m => m.CompanyTypeId);
        // 2. Navigation properties
        builder.HasOne(m => m.CompanyType).WithMany().HasForeignKey(m => m.CompanyTypeId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.OwnsOne(m => m.Name, static owned => owned.ConfigureOwnedEntity());
        // 3. Propery specification
        builder.Property(m => m.CreationDate).HasComment("The date this company was established or founded.").HasColumnName("DOB");
        builder.Property(m => m.CreationCity).HasComment("The city the company was established or founded.");
        builder.Property(m => m.CreationState).HasComment("The state the company was established or founded.");
        builder.Property(m => m.CreationCountry).HasComment("The country the company was established or founded.");
        builder.Property(m => m.Description).HasComment("A description of the entity.").HasMaxLength(2500);
        // 4. Other
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<CompanyEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.CreationCity).IsNotNullOrWhiteSpace();
        validator.Property(m => m.CreationState).IsNotNullOrWhiteSpace();
        validator.Property(m => m.CreationCountry).IsNotNullOrWhiteSpace();
        validator.Property(m => m.Description).IsNotNullOrWhiteSpace().HasLengthLessThanOrEqualTo(1250);
    }
}
