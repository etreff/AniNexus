using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities.Configuration;

internal class UserBannedEntityConfiguration : BaseConfiguration<UserBannedEntity>
{
    public override void Configure(EntityTypeBuilder<UserBannedEntity> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.UserId);

        builder.HasOne(x => x.User).WithMany(x => x.BanReasons).HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
