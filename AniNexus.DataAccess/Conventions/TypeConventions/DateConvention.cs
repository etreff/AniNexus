using Microsoft.EntityFrameworkCore;
using AniNexus.DataAccess.Converters;

namespace AniNexus.DataAccess.Conventions;

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

        builder.Properties<FuzzyDate>()
            .HaveConversion<FuzzyDateConverter, FuzzyDateComparer>();

        builder.Properties<FuzzyDate?>()
            .HaveConversion<NullableFuzzyDateConverter, NullableFuzzyDateComparer>();
    }
}
