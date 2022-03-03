namespace AniNexus.Data.Conventions;

internal sealed class StringListConvention : ITypeConvention<IList<string>>
{
    public void Configure(ModelConfigurationBuilder builder, PropertiesConfigurationBuilder<IList<string>> properties)
    {
        properties.HaveColumnType("varchar(max)");
    }
}
