using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Conventions;

internal sealed class StringConvention : ITypeConvention<string>
{
    public void Configure(ModelConfigurationBuilder builder, PropertiesConfigurationBuilder<string> properties)
    {
        properties.UseCollation(Collation.CaseSensitive);
    }
}

internal sealed class NullableStringConvention : ITypeConvention<string?>
{
    public void Configure(ModelConfigurationBuilder builder, PropertiesConfigurationBuilder<string?> properties)
    {
        properties.UseCollation(Collation.CaseSensitive);
    }
}
