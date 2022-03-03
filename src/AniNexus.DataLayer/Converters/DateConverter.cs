using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AniNexus.Data.Converters;

/// <summary>
/// Converts <see cref="Date"/> to <see cref="string"/>
/// and vice versa.
/// </summary>
public class DateConverter : ValueConverter<Date, string>
{
    /// <summary>
    /// Creates a new <see cref="DateConverter"/> instance.
    /// </summary>
    public DateConverter()
        : base(static d => d.ToString(), static d => Date.Parse(d, '.'))
    {
    }
}

/// <summary>
/// Converts <see cref="Date"/> to <see cref="string"/>
/// and vice versa.
/// </summary>
public class NullableDateConverter : ValueConverter<Date?, string?>
{
    /// <summary>
    /// Creates a new <see cref="NullableDateConverter"/> instance.
    /// </summary>
    public NullableDateConverter()
        : base(static d => d.HasValue ? d.Value.ToString() : null, static d => d != null ? Date.Parse(d, '.') : null)
    {
    }
}

/// <summary>
/// Compares <see cref="Date" />.
/// </summary>
public class DateComparer : ValueComparer<Date>
{
    /// <summary>
    /// Creates a new <see cref="DateComparer"/> instance.
    /// </summary>
    public DateComparer()
        : base(static (d1, d2) => d1.Equals(d2), static d => d.GetHashCode())
    {
    }
}

/// <summary>
/// Compares <see cref="Date" />.
/// </summary>
public class NullableDateComparer : ValueComparer<Date?>
{
    /// <summary>
    /// Creates a new <see cref="NullableDateComparer"/> instance.
    /// </summary>
    public NullableDateComparer()
        : base(static (d1, d2) => (!d1.HasValue && !d2.HasValue) || (d1.HasValue && d2.HasValue && d1.Value == d2.Value), static d => d.HasValue ? d.GetHashCode() : 0)
    {
    }
}
