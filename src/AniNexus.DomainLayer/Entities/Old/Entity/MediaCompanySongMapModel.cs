#if SONGMODEL
using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a company and the song they had roles in.
/// </summary>
public class MediaCompanySongMapModel : IEntityConfiguration<MediaCompanySongMapModel>
{
    /// <summary>
    /// The company Id.
    /// </summary>
    /// <seealso cref="MediaCompanyModel">
    public int CompanyId { get; set; }

    /// <summary>
    /// The song Id.
    /// </summary>
    /// <seealso cref="SongModel"/>
    public int SongId { get; set; }

    /// <summary>
    /// The role Id.
    /// </summary>
    /// <seealso cref="ECompanyRole"/>
    /// <seealso cref="CompanyRoleTypeModel"/>
    public int RoleId { get; set; }

#region Navigation Properties
    /// <summary>
    /// The company.
    /// </summary>
    public MediaCompanyModel Company { get; set; } = default!;

    /// <summary>
    /// The song.
    /// </summary>
    public SongModel Song { get; set; } = default!;

    /// <summary>
    /// The role the company played in the media.
    /// </summary>
    public CompanyRoleTypeModel Role { get; set; } = default!;
#endregion

    public static void Configure(EntityTypeBuilder<MediaCompanySongMapModel> builder)
    {
        builder.ToTable("CompanySongMap");

        builder.HasKey(m => new { m.CompanyId, m.SongId, m.RoleId });
        builder.HasIndex(m => m.SongId);
        builder.HasIndex(m => m.RoleId);

        builder.HasOne(m => m.Company).WithMany(m => m.SongRoles).HasForeignKey(m => m.CompanyId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Song).WithMany(m => m.Roles).HasForeignKey(m => m.SongId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        
        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Song).AutoInclude();
        builder.Navigation(m => m.Company).AutoInclude();
        builder.Navigation(m => m.Role).AutoInclude();
    }
}

#endif