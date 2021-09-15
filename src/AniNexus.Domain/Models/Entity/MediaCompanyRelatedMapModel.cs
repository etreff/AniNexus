using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between related companies.
/// </summary>
public class MediaCompanyRelatedMapModel : IEntityTypeConfiguration<MediaCompanyRelatedMapModel>
{
    /// <summary>
    /// The Id of the company.
    /// </summary>
    /// <seealso cref="MediaCompanyModel"/>
    public int CompanyId { get; set; }

    /// <summary>
    /// The Id of the related company.
    /// </summary>
    /// <seealso cref="MediaCompanyModel"/>
    public int RelatedCompanyId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The company.
    /// </summary>
    public MediaCompanyModel Company { get; set; } = default!;

    /// <summary>
    /// The related company.
    /// </summary>
    public MediaCompanyModel RelatedCompany { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaCompanyRelatedMapModel> builder)
    {
        builder.ToTable("CompanyRelatedMap");

        builder.HasKey(m => new { m.CompanyId, m.RelatedCompanyId });
        builder.HasIndex(m => m.RelatedCompanyId);

        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths since the mapping
        // points to two entities in the same table.
        builder.HasOne(m => m.Company).WithMany(m => m.RelatedCompanies).HasForeignKey(m => m.CompanyId).IsRequired().OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.RelatedCompany).WithMany().HasForeignKey(m => m.RelatedCompanyId).IsRequired().OnDelete(DeleteBehavior.NoAction);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Company).AutoInclude();
        builder.Navigation(m => m.RelatedCompany).AutoInclude();
    }
}
