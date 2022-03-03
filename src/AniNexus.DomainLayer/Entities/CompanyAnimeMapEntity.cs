using AniNexus.Models;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a mapping between a company and the anime they had roles in.
/// </summary>
public class CompanyAnimeMapEntity : Entity<CompanyAnimeMapEntity>
{
    /// <summary>
    /// The company Id.
    /// </summary>
    /// <seealso cref="CompanyEntity"/>
    public Guid CompanyId { get; set; }

    /// <summary>
    /// The anime Id.
    /// </summary>
    public Guid AnimeId { get; set; }

    /// <summary>
    /// The anime release Id, if applicable. If set, this company will only be credited for
    /// this specific release.
    /// </summary>
    public Guid? ReleaseId { get; set; }

    /// <summary>
    /// The role Id.
    /// </summary>
    /// <seealso cref="ECompanyRole"/>
    /// <seealso cref="CompanyRoleTypeEntity"/>
    public byte RoleId { get; set; }

    /// <summary>
    /// The company.
    /// </summary>
    public CompanyEntity Company { get; set; } = default!;

    /// <summary>
    /// The anime.
    /// </summary>
    public AnimeEntity Anime { get; set; } = default!;

    /// <summary>
    /// The anime release, if applicable.
    /// </summary>
    public AnimeReleaseEntity? Release { get; set; }

    /// <summary>
    /// The role the company played in the media.
    /// </summary>
    public CompanyRoleTypeEntity Role { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<CompanyAnimeMapEntity> builder)
    {
        // 1. Index specification
        builder.HasIndex(m => new { m.CompanyId, m.AnimeId, m.ReleaseId, m.RoleId }).IsUnique();
        builder.HasIndex(m => m.AnimeId);
        builder.HasIndex(m => m.ReleaseId).HasNotNullFilter();
        builder.HasIndex(m => m.RoleId);
        // 2. Navigation properties
        builder.HasOne(m => m.Company).WithMany(m => m.AnimeRoles).HasForeignKey(m => m.CompanyId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Anime).WithMany(m => m.Companies).HasForeignKey(m => m.AnimeId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // Handled by EF trigger - SqlServer will falsely flag multiple cascade paths.
        builder.HasOne(m => m.Release).WithMany(m => m.Companies).HasForeignKey(m => m.ReleaseId).IsRequired(false).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        // 3. Propery specification
        // 4. Other
    }
}
