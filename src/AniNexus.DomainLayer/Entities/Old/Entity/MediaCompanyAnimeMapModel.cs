using AniNexus.Models;
using System.ComponentModel.DataAnnotations;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a company and the anime they had roles in.
/// </summary>
public class MediaCompanyAnimeMapModel : IEntityTypeConfiguration<MediaCompanyAnimeMapModel>, IValidatableObject
{
    /// <summary>
    /// The company Id.
    /// </summary>
    /// <seealso cref="MediaCompanyModel"/>
    public int CompanyId { get; set; }

    /// <summary>
    /// The anime Id.
    /// </summary>
    /// <seealso cref="AnimeModel"/>
    public int AnimeId { get; set; }

    /// <summary>
    /// The anime release Id, if applicable.
    /// </summary>
    /// <seealso cref="AnimeReleaseModel"/>
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
    /// The anime.
    /// </summary>
    public AnimeModel Anime { get; set; } = default!;

    /// <summary>
    /// The anime release, if applicable.
    /// </summary>
    public AnimeReleaseModel? Release { get; set; }

    /// <summary>
    /// The role the company played in the media.
    /// </summary>
    public CompanyRoleTypeEntity Role { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaCompanyAnimeMapModel> builder)
    {
        builder.ToTable("CompanyAnimeMap");

        builder.HasKey(m => new { m.CompanyId, m.AnimeId, m.RoleId });
        builder.HasIndex(m => m.AnimeId);
        builder.HasIndex(m => m.RoleId);
        builder.HasIndex(m => m.ReleaseId).HasFilter("[ReleaseId] IS NOT NULL");

        builder.HasOne(m => m.Company).WithMany(m => m.AnimeRoles).HasForeignKey(m => m.CompanyId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Anime).WithMany(m => m.Companies).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths.
        builder.HasOne(m => m.Release).WithMany().HasForeignKey(m => m.ReleaseId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Anime).AutoInclude();
        builder.Navigation(m => m.Company).AutoInclude();
        builder.Navigation(m => m.Role).AutoInclude();
        builder.Navigation(m => m.Release).AutoInclude();
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ReleaseId.HasValue && RoleId != (int)ECompanyRole.Publisher)
        {
            yield return new ValidationResult("The release Id should only be set if the role is publisher/licensee.", new[] { nameof(ReleaseId) });
        }
    }
}
