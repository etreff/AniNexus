using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AniNexus.Domain.Converters;

/// <summary>
/// Converts <see cref="TimeOnly"/> to <see cref="TimeSpan"/>
/// and vice versa.
/// </summary>
/// <remarks>
/// This is a workaround until EFCore implements a built-in converter.
/// See https://github.com/dotnet/efcore/issues/24507#issuecomment-891034323
/// </remarks>
public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
{
    /// <summary>
    /// Creates a new <see cref="TimeOnlyConverter"/> instance.
    /// </summary>
    public TimeOnlyConverter()
        : base(static d => d.ToTimeSpan(), static d => TimeOnly.FromTimeSpan(d))
    {
    }
}

/// <summary>
/// Converts <see cref="TimeOnly"/> to <see cref="TimeSpan"/>
/// and vice versa.
/// </summary>
/// <remarks>
/// This is a workaround until EFCore implements a built-in converter.
/// See https://github.com/dotnet/efcore/issues/24507#issuecomment-891034323
/// </remarks>
public class NullableTimeOnlyConverter : ValueConverter<TimeOnly?, TimeSpan?>
{
    /// <summary>
    /// Creates a new <see cref="NullableTimeOnlyConverter"/> instance.
    /// </summary>
    public NullableTimeOnlyConverter()
        : base(static d => d.HasValue ? d.Value.ToTimeSpan() : null, static d => d.HasValue ? TimeOnly.FromTimeSpan(d.Value) : null)
    {
    }
}

/// <summary>
/// Compares <see cref="TimeOnly" />.
/// </summary>
public class TimeOnlyComparer : ValueComparer<TimeOnly>
{
    /// <summary>
    /// Creates a new <see cref="TimeOnlyComparer"/> instance.
    /// </summary>
    public TimeOnlyComparer()
        : base(static (d1, d2) => d1 == d2 && d1.Ticks == d2.Ticks, static d => d.GetHashCode())
    {
    }
}

/// <summary>
/// Compares <see cref="TimeOnly" />.
/// </summary>
public class NullableTimeOnlyComparer : ValueComparer<TimeOnly?>
{
    /// <summary>
    /// Creates a new <see cref="NullableTimeOnlyComparer"/> instance.
    /// </summary>
    public NullableTimeOnlyComparer()
        : base(static (d1, d2) => (!d1.HasValue && !d2.HasValue) || (d1.HasValue && d2.HasValue && d1.Value == d2.Value), static d => d.HasValue ? d.GetHashCode() : 0)
    {
    }
}
