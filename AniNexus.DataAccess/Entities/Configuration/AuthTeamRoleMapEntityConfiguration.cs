using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities.Configuration;

internal sealed class AuthTeamRoleMapEntityConfiguration : BaseConfiguration<AuthTeamRoleMapEntity>
{
    public override void Configure(EntityTypeBuilder<AuthTeamRoleMapEntity> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => new { x.TeamId, x.RoleId }).IsUnique();

        builder.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Team).WithMany().HasForeignKey(x => x.TeamId).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
