using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities.Configuration;

internal sealed class AuthTeamUserMapEntityConfiguration : BaseConfiguration<AuthTeamUserMapEntity, long>
{
    public override void Configure(EntityTypeBuilder<AuthTeamUserMapEntity> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => new { x.UserId, x.TeamId }).IsUnique();

        builder.HasOne(x => x.User).WithMany(x => x.Teams).HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Team).WithMany().HasForeignKey(x => x.TeamId).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
