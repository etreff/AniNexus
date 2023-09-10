using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities.Configuration;

internal sealed class AuthTeamEntityConfiguration : BaseConfiguration<AuthTeamEntity>
{
    public override void Configure(EntityTypeBuilder<AuthTeamEntity> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.Name).IsUnique();
    }
}
