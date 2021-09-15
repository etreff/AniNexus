﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AniNexus.Domain.Converters;

/// <summary>
/// Converts <see cref="DateOnly"/> to <see cref="DateTime"/>
/// and vice versa.
/// </summary>
/// <remarks>
/// This is a workaround until EFCore implements a built-in converter.
///
/// See https://github.com/dotnet/efcore/issues/24507#issuecomment-891034323
/// </remarks>
public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter()
        : base(static d => d.ToDateTime(TimeOnly.MinValue), static d => DateOnly.FromDateTime(d))
    {
    }
}

/// <summary>
/// Converts <see cref="DateOnly"/> to <see cref="DateTime"/>
/// and vice versa.
/// </summary>
/// <remarks>
/// This is a workaround until EFCore implements a built-in converter.
///
/// See https://github.com/dotnet/efcore/issues/24507#issuecomment-891034323
/// </remarks>
public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
{
    public NullableDateOnlyConverter()
        : base(static d => d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : null, static d => d.HasValue ? DateOnly.FromDateTime(d.Value) : null)
    {
    }
}

/// <summary>
/// Compares <see cref="DateOnly" />.
/// </summary>
public class DateOnlyComparer : ValueComparer<DateOnly>
{
    public DateOnlyComparer()
        : base(static (d1, d2) => d1 == d2 && d1.DayNumber == d2.DayNumber, static d => d.GetHashCode())
    {

    }
}

/// <summary>
/// Compares <see cref="DateOnly" />.
/// </summary>
public class NullableDateOnlyComparer : ValueComparer<DateOnly?>
{
    public NullableDateOnlyComparer()
        : base(static (d1, d2) => (!d1.HasValue && !d2.HasValue) || (d1.HasValue && d2.HasValue && d1.Value == d2.Value), static d => d.HasValue ? d.GetHashCode() : 0)
    {

    }
}
