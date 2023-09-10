using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AniNexus.DataAccess.Conventions;

/// <summary>
/// A converter that forces <see cref="DateTime"/> objects to be UTC times.
/// </summary>
internal sealed class DateTimeUtcValueConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeUtcValueConverter(ConverterMappingHints? mappingHints = null)
        : base(x => x.ToUniversalTime(), x => DateTime.SpecifyKind(x, DateTimeKind.Utc), mappingHints)
    {
    }
}

/// <summary>
/// A converter that forces nullable <see cref="DateTime"/> objects to be UTC times.
/// </summary>
internal sealed class NullableDateTimeUtcValueConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableDateTimeUtcValueConverter(ConverterMappingHints? mappingHints = null)
        : base(x => x != null ? x.Value.ToUniversalTime() : null, x => x != null ? DateTime.SpecifyKind(x.Value, DateTimeKind.Utc) : null, mappingHints)
    {
    }
}
