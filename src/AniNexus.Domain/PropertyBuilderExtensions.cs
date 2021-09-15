using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.Domain;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<DateTime> HasComputedSqlDate(this PropertyBuilder<DateTime> builder, bool update = false)
    {
        builder.HasComputedColumnSql("getutcdate()");

        if (!update)
        {
            builder.ValueGeneratedOnAdd();
        }
        else
        {
            builder.ValueGeneratedOnAddOrUpdate();
        }

        return builder;
    }
}
