using AniNexus.Data.Converters;

namespace AniNexus.Data.Conventions;

internal sealed class DateConvention : ITypeConvention
{
    public void Configure(ModelConfigurationBuilder builder)
    {
        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter, DateOnlyComparer>()
            .HaveColumnType("date");

        builder.Properties<DateOnly?>()
            .HaveConversion<NullableDateOnlyConverter, NullableDateOnlyComparer>()
            .HaveColumnType("date");

        builder.Properties<TimeOnly>()
            .HaveConversion<TimeOnlyConverter, TimeOnlyComparer>();

        builder.Properties<TimeOnly?>()
            .HaveConversion<NullableTimeOnlyConverter, TimeOnlyComparer>();

        // YYYY.MM.DD
        builder.Properties<Date>()
            .HaveConversion<DateConverter, DateComparer>()
            .HaveColumnType("char(10)")
            .AreFixedLength(true);

        builder.Properties<Date?>()
            .HaveConversion<NullableDateConverter, NullableDateComparer>()
            .HaveColumnType("char(10)")
            .AreFixedLength(true);
    }
}
