using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AniNexus.DataAccess.Conventions;

internal sealed class UtcDateTimeConvention : ITypeConvention<DateTime>
{
    public void Configure(ModelConfigurationBuilder builder, PropertiesConfigurationBuilder<DateTime> properties)
    {
        properties.HaveConversion<UtcDateTimeConvention>();
    }
}

internal sealed class NullableUtcDateTimeConvention : ITypeConvention<DateTime?>
{
    public void Configure(ModelConfigurationBuilder builder, PropertiesConfigurationBuilder<DateTime?> properties)
    {
        properties.HaveConversion<NullableUtcDateTimeConvention>();
    }
}
