#if SONGMODEL
using AniNexus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a company and the album they had roles in.
/// </summary>
public class MediaCompanyAlbumMapModel : IEntityConfiguration<MediaCompanyAlbumMapModel>
{
    /// <summary>
    /// The company Id.
    /// </summary>
    /// <seealso cref="MediaCompanyModel"/>
    public int CompanyId { get; set; }

    /// <summary>
    /// The albumId Id.
    /// </summary>
    /// <seealso cref="AlbumModel"/>
    public int AlbumId { get; set; }

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
    /// The album.
    /// </summary>
    public AlbumModel Album { get; set; } = default!;

    /// <summary>
    /// The role the company played in the media.
    /// </summary>
    public CompanyRoleTypeModel Role { get; set; } = default!;
#endregion

    public static void Configure(EntityTypeBuilder<MediaCompanyAlbumMapModel> builder)
    {
        builder.ToTable("CompanyAlbumMap");

        builder.HasKey(m => new { m.CompanyId, m.AlbumId, m.RoleId });
        builder.HasKey(m => m.AlbumId);
        builder.HasKey(m => m.RoleId);

        builder.HasOne(m => m.Company).WithMany(m => m.AlbumRoles).HasForeignKey(m => m.CompanyId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Album).WithMany(m => m.Roles).HasForeignKey(m => m.AlbumId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        
        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Album).AutoInclude();
        builder.Navigation(m => m.Company).AutoInclude();
        builder.Navigation(m => m.Role).AutoInclude();
    }
}

#endif