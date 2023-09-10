using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AniNexus.DataAccess.Converters;

/// <summary>
/// Converts <see cref="FuzzyDate"/> to <see cref="string"/>
/// and vice versa.
/// </summary>
public class FuzzyDateConverter : ValueConverter<FuzzyDate, string>
{
    /// <summary>
    /// Creates a new <see cref="FuzzyDateConverter"/> instance.
    /// </summary>
    public FuzzyDateConverter()
        : base(static d => d.ToString(), static d => FuzzyDate.Parse(d, '.'))
    {
    }
}

/// <summary>
/// Converts <see cref="FuzzyDate"/> to <see cref="string"/>
/// and vice versa.
/// </summary>
public class NullableFuzzyDateConverter : ValueConverter<FuzzyDate?, string?>
{
    /// <summary>
    /// Creates a new <see cref="NullableFuzzyDateConverter"/> instance.
    /// </summary>
    public NullableFuzzyDateConverter()
        : base(static d => d.HasValue ? d.Value.ToString() : null, static d => d != null ? FuzzyDate.Parse(d, '.') : null)
    {
    }
}

/// <summary>
/// Compares <see cref="FuzzyDate" />.
/// </summary>
public class FuzzyDateComparer : ValueComparer<FuzzyDate>
{
    /// <summary>
    /// Creates a new <see cref="FuzzyDateComparer"/> instance.
    /// </summary>
    public FuzzyDateComparer()
        : base(static (d1, d2) => d1.Equals(d2), static d => d.GetHashCode())
    {
    }
}

/// <summary>
/// Compares <see cref="FuzzyDate" />.
/// </summary>
public class NullableFuzzyDateComparer : ValueComparer<FuzzyDate?>
{
    /// <summary>
    /// Creates a new <see cref="NullableFuzzyDateComparer"/> instance.
    /// </summary>
    public NullableFuzzyDateComparer()
        : base(static (d1, d2) => (!d1.HasValue && !d2.HasValue) || (d1.HasValue && d2.HasValue && d1.Value.Equals(d2.Value)), static d => d.HasValue ? d.GetHashCode() : 0)
    {
    }
}
