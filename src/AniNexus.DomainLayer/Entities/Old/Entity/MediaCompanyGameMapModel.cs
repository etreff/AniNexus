using AniNexus.Models;

namespace AniNexus.Domain.Models;

/// <summary>
/// Models a mapping between a company and the game they had roles in.
/// </summary>
public class MediaCompanyGameMapModel : IEntityTypeConfiguration<MediaCompanyGameMapModel>
{
    /// <summary>
    /// The company Id.
    /// </summary>
    /// <seealso cref="MediaCompanyModel"/>
    public int CompanyId { get; set; }

    /// <summary>
    /// The game Id.
    /// </summary>
    /// <seealso cref="GameModel"/>
    public int GameId { get; set; }

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
    /// The game.
    /// </summary>
    public GameModel Game { get; set; } = default!;

    /// <summary>
    /// The role the company played in the media.
    /// </summary>
    public CompanyRoleTypeEntity Role { get; set; } = default!;
    #endregion

    public void Configure(EntityTypeBuilder<MediaCompanyGameMapModel> builder)
    {
        builder.ToTable("CompanyGameMap");

        builder.HasKey(m => new { m.CompanyId, m.GameId, m.RoleId });
        builder.HasIndex(m => m.GameId);
        builder.HasIndex(m => m.RoleId);

        builder.HasOne(m => m.Company).WithMany(m => m.GameRoles).HasForeignKey(m => m.CompanyId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Game).WithMany(m => m.Companies).HasForeignKey(m => m.GameId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(m => m.Role).WithMany().HasForeignKey(m => m.RoleId).IsRequired().OnDelete(DeleteBehavior.Restrict);

        // Justification - maps always auto include navigation properties.
        builder.Navigation(m => m.Game).AutoInclude();
        builder.Navigation(m => m.Company).AutoInclude();
        builder.Navigation(m => m.Role).AutoInclude();
    }
}
