using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AniNexus.Domain.Converters;

/// <summary>
/// Converts <see cref="Guid"/> to <see cref="string"/>
/// and vice versa.
/// </summary>
public class GuidConverter : ValueConverter<Guid, string>
{
    public GuidConverter()
        : base(static d => d.ToString(), static d => Guid.Parse(d))
    {
    }
}

/// <summary>
/// Converts <see cref="Guid"/> to <see cref="string"/>
/// and vice versa.
/// </summary>
public class NullableGuidConverter : ValueConverter<Guid?, string?>
{
    public NullableGuidConverter()
        : base(static d => d.HasValue ? d.Value.ToString() : null, static d => d != null ? Guid.Parse(d) : null)
    {
    }
}

/// <summary>
/// Compares <see cref="Guid" />.
/// </summary>
public class GuidComparer : ValueComparer<Guid>
{
    public GuidComparer()
        : base(static (d1, d2) => d1.Equals(d2), static d => d.GetHashCode())
    {

    }
}

/// <summary>
/// Compares <see cref="Guid" />.
/// </summary>
public class NullableGuidComparer : ValueComparer<Guid?>
{
    public NullableGuidComparer()
        : base(static (d1, d2) => (!d1.HasValue && !d2.HasValue) || (d1.HasValue && d2.HasValue && d1.Value == d2.Value), static d => d.HasValue ? d.GetHashCode() : 0)
    {

    }
}
