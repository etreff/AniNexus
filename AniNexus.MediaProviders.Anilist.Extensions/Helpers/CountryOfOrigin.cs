namespace AniNexus.MediaProviders.Anilist;

/// <summary>
/// Helper class for working with Anilist country of origin.
/// </summary>
internal readonly struct CountryOfOrigin : IEquatable<CountryOfOrigin>, IEquatable<string>
{
    public readonly string Origin { get; }

    public static CountryOfOrigin Japan { get; } = new("jp");
    public static CountryOfOrigin China { get; } = new("cn");

    private CountryOfOrigin(string origin)
    {
        Origin = origin;
    }

    public readonly bool Equals(CountryOfOrigin other)
    {
        return Equals(other.Origin);
    }

    public readonly bool Equals(string? other)
    {
        return other is not null && StringComparer.OrdinalIgnoreCase.Equals(Origin, other);
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is string s)
        {
            return Equals(s);
        }

        if (obj is CountryOfOrigin o)
        {
            return Equals(o);
        }

        return false;
    }

    public override readonly int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Origin);
    }

    public override readonly string ToString()
    {
        return Origin;
    }

    public static bool operator ==(CountryOfOrigin a, CountryOfOrigin b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(CountryOfOrigin a, CountryOfOrigin b)
    {
        return !(a == b);
    }

    public static bool operator ==(CountryOfOrigin a, string? b)
    {
        if (b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(CountryOfOrigin a, string? b)
    {
        return !(a == b);
    }

    public static bool operator ==(string? a, CountryOfOrigin b)
    {
        if (a is null)
        {
            return false;
        }

        return b.Equals(a);
    }

    public static bool operator !=(string? a, CountryOfOrigin b)
    {
        return !(a == b);
    }
}
