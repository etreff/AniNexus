using AniNexus.Models;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a company and the manga they had roles in.
/// </summary>
public class MediaCompanyMangaMapModel : IEntityTypeConfiguration<MediaCompanyMangaMapModel>
{
    /// <summary>
    /// The company Id.
    /// </summary>
    /// <see cref="MediaCompanyModel"/>
    public int CompanyId { get; set; }

    /// <summary>
    /// The manga Id.
    /// </summary>
    /// <seealso cref="MangaModel"/>
    public int MangaId { get; set; }

    /// <summary>
    /// The manga release Id, if applicable.
    /// </summary>
    /// <seealso cref="MangaReleaseModel"/>
    public int? ReleaseId { get; set; }

    /// <summary>
    /// The role Id.
    /// </summary>
    /// <seealso cref="ECompanyRole"/>
    /// <seealso cref="CompanyRoleTypeEntity"/>
    public int RoleId { get; set; }

    #region Navigation Properties
    /// <summary>
    /// The company.
    /// </summary>
    public MediaCompanyModel Company { get; set; } = default!;

    /// <summary>
    /// The manga.
    /// </summary>
    public MangaModel Manga { get; set; } = default!;

    /// <summary>
    /// The manga release, if applicable.
    /// </summary>
    public MangaReleaseModel? Release { get; set; }

    /// <summary>
    /// The role the company played in the media.
    /// </summary>
    public CompanyRoleTypeEntity Role { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaCompanyMangaMapModel> builder)
    {
        builder.ToTable("CompanyMangaMap");

        builder.HasKey(m => new { m.CompanyId, m.MangaId, m.RoleId });
        builder.HasIndex(m => m.MangaId);
        builder.HasIndex(m => m.RoleId);
        builder.HasIndex(m => m.ReleaseId);

        builder.HasOne(m => m.Company).WithMany(m => m.MangaRoles).HasForeignKey(m => m.CompanyId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Manga).WithMany(m => m.Companies).HasForeignKey(m => m.MangaId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths.
        builder.HasOne(m => m.Release).WithMany().HasForeignKey(m => m.ReleaseId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Manga).AutoInclude();
        builder.Navigation(m => m.Company).AutoInclude();
        builder.Navigation(m => m.Role).AutoInclude();
        builder.Navigation(m => m.Release).AutoInclude();
    }
}
