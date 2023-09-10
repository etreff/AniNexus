using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Entities.Configuration;

internal sealed class UserEntityConfiguration : BaseConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => x.Username).IsUnique();

        builder.Property(x => x.Username).UseCollation(Collation.CaseInsensitive);
        builder.Property(x => x.Password).IsFixedLength(84);
        builder.Property(x => x.PasswordResetCode!).IsFixedLength(172);
    }

    protected override string GetTableName()
    {
        return "Users";
    }
}
