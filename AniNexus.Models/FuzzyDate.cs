using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AniNexus;

/// <summary>
/// Represents a date that can be incomplete.
/// </summary>
public readonly struct FuzzyDate : IEquatable<FuzzyDate>, IComparable<FuzzyDate>
{
    /// <summary>
    /// The minimum supported <see cref="FuzzyDate"/> value.
    /// </summary>
    public static FuzzyDate MinValue => new(0, 1, 0);

    /// <summary>
    /// The maximum supported <see cref="FuzzyDate"/> value.
    /// </summary>
    public static FuzzyDate MaxValue => new(int.MaxValue, 12, 31);

    /// <summary>
    /// The year component.
    /// </summary>
    public readonly int Year { get; }

    /// <summary>
    /// The month component.
    /// </summary>
    public readonly int Month { get; }

    /// <summary>
    /// The day component.
    /// </summary>
    public readonly int Day { get; }

    /// <summary>
    /// Creates a new <see cref="FuzzyDate"/> instance.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> is 0.</exception>
    public FuzzyDate(int year)
        : this(year, 0)
    {
    }

    /// <summary>
    /// Creates a new <see cref="FuzzyDate"/> instance.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="year"/> and <paramref name="month"/> are both 0.</exception>
    /// <remarks>
    /// If month is higher than 12 it will be set to 12.
    /// This class does not check for date validity.
    /// </remarks>
    public FuzzyDate(int year, int month)
        : this(year, month, 0)
    {
    }

    /// <summary>
    /// Creates a new <see cref="FuzzyDate"/> instance.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <param name="day">The day.</param>
    /// <remarks>
    /// If month and day are higher than 12 and 31, they will be set to those respective values.
    /// This class does not check for date validity.
    /// </remarks>
    private FuzzyDate(int year, int month, int day)
    {
        Year = Math.Max(0, year);
        Month = Math.Max(0, Math.Min(12, month));
        Day = Math.Max(0, Math.Min(31, day));
    }

    /// <summary>
    /// Attempts to convert this <see cref="FuzzyDate"/> instance into a <see cref="DateTime"/> object.
    /// This conversion will only succeed if <see cref="Year"/>, <see cref="Month"/>, and <see cref="Day"/>
    /// are all valid.
    /// </summary>
    /// <param name="result">The converted <see cref="DateTime"/> object on success.</param>
    public bool TryGetDateTime([NotNullWhen(true)] out DateTime? result)
    {
        // We have no good way to validate whether the combination of Year/Month/Day in this
        // Date object make a real date, and the DateTime/DateTimeOffset objects do not expose
        // the methods of validation that are used in the constructor. The best we can do is
        // wrap the constructor in a try-catch.
        try
        {
            if (Year > 0 && Month > 0 && Day > 0)
            {
                result = new DateTime(Year, Month, Day, 0, 0, 0, DateTimeKind.Utc);
                return true;
            }
        }
        catch
        {
            // Suppress
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Returns a <see cref="FuzzyDate"/> object that represents the current local date.
    /// </summary>
    public static FuzzyDate Now()
    {
        return DateTime.Now;
    }

    /// <summary>
    /// Returns a <see cref="FuzzyDate"/> object that represents the current UTC date.
    /// </summary>
    public static FuzzyDate UtcNow()
    {
        return DateTime.UtcNow;
    }

    /// <summary>
    /// Parses an eight digit number into a <see cref="FuzzyDate"/> object.
    /// </summary>
    /// <param name="date">The number to parse.</param>
    /// <exception cref="FormatException">The input is not a valid date format.</exception>
    public static FuzzyDate Parse(int date)
    {
        if (!TryParse(date, out var result))
        {
            throw new FormatException("The input is not in a valid Date format.");
        }

        return result.Value;
    }

    /// <summary>
    /// Parses an eight digit number into a <see cref="FuzzyDate"/> object. If the date cannot be parsed
    /// or equals 0, <see langword="null"/> will be returned.
    /// </summary>
    /// <param name="date">The number to parse.</param>
    public static FuzzyDate? ParseOrNull(int date)
    {
        if (!TryParse(date, out var result))
        {
            return null;
        }

        return result.Value;
    }

    /// <summary>
    /// Parses a string into a <see cref="FuzzyDate"/> object.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="delimiter">The delimiter of the date components.</param>
    public static FuzzyDate Parse(string s, char delimiter = '.')
        => Parse(s.AsSpan(), delimiter);

    /// <summary>
    /// Parses a <see cref="ReadOnlySpan{T}"/> into a <see cref="FuzzyDate"/> object.
    /// </summary>
    /// <param name="span">The characters to parse.</param>
    /// <param name="delimiter">The delimiter of the date components.</param>
    /// <exception cref="FormatException"><paramref name="span"/> is not a valid Date format.</exception>
    public static FuzzyDate Parse(ReadOnlySpan<char> span, char delimiter = '.')
    {
        if (!TryParse(span, delimiter, out var result))
        {
            throw new FormatException("The input is not in a valid Date format.");
        }

        return result.Value;
    }

    /// <summary>
    /// Attempts to parse a string into a <see cref="FuzzyDate"/> object.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="result">The date if parsing is successful.</param>
    /// <returns>Whether parsing succeeded.</returns>
    public static bool TryParse(string s, [NotNullWhen(true)] out FuzzyDate? result)
        => TryParse(s, '.', out result);

    /// <summary>
    /// Attempts to parse a string into a <see cref="FuzzyDate"/> object.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="delimiter">The delimiter that separates the date components.</param>
    /// <param name="result">The date if parsing is successful.</param>
    /// <returns>Whether parsing succeeded.</returns>
    public static bool TryParse(string s, char delimiter, [NotNullWhen(true)] out FuzzyDate? result)
        => TryParse(s.AsSpan(), delimiter, out result);

    /// <summary>
    /// Attempts to parse a region of chars into a <see cref="FuzzyDate"/> object.
    /// </summary>
    /// <param name="span">The chars to parse.</param>
    /// <param name="result">The date if parsing is successful.</param>
    /// <returns>Whether parsing succeeded.</returns>
    public static bool TryParse(ReadOnlySpan<char> span, [NotNullWhen(true)] out FuzzyDate? result)
        => TryParse(span, '.', out result);

    /// <summary>
    /// Attempts to parse a region of chars into a <see cref="FuzzyDate"/> object.
    /// </summary>
    /// <param name="span">The chars to parse.</param>
    /// <param name="delimiter">The delimiter that separates the date components.</param>
    /// <param name="result">The date if parsing is successful.</param>
    /// <returns>Whether parsing succeeded.</returns>
    public static bool TryParse(ReadOnlySpan<char> span, char delimiter, [NotNullWhen(true)] out FuzzyDate? result)
    {
        Span<int> dateParts = stackalloc int[3] { 0, 0, 0 };
        int index = 0;
        foreach (var part in span.Split(delimiter))
        {
            if (index < dateParts.Length)
            {
                int n = int.Parse(part);
                if (n < 0)
                {
                    result = null;
                    return false;
                }
                dateParts[index++] = n;
            }
            else
            {
                result = null;
                return false;
            }
        }

        result = new FuzzyDate(dateParts[0], dateParts[1], dateParts[2]);
        return true;
    }

    /// <summary>
    /// Attempts to parse an eight digit number into a <see cref="FuzzyDate"/> object.
    /// </summary>
    /// <param name="date">The number to parse.</param>
    /// <param name="result">The date if parsing is successful.</param>
    /// <returns>Whether parsing succeeded.</returns>
    public static bool TryParse(int date, [NotNullWhen(true)] out FuzzyDate? result)
    {
        result = null;

        if (date == 0)
        {
            // Reperesents an unknown date.
            return false;
        }

        var dateString = date.ToString().AsSpan();
        if (dateString.Length != 8)
        {
            return false;
        }

        int year = int.Parse(dateString.Slice(0, 4));
        int month = int.Parse(dateString.Slice(4, 2));
        int day = int.Parse(dateString.Slice(6, 2));
        result = new FuzzyDate(year, month, day);

        return true;
    }

    /// <summary>
    /// Attempts to create a <see cref="FuzzyDate"/> object with the 3 specified components.
    /// </summary>
    /// <param name="year">The year component.</param>
    /// <param name="month">The month component.</param>
    /// <param name="day">The day component.</param>
    /// <param name="result">The date if all 3 components can be used to create a valid <see cref="FuzzyDate"/> object.</param>
    public static bool TryCreate(int? year, int? month, int? day, [NotNullWhen(true)] out FuzzyDate? result)
    {
        if (IsValid(year, month, day))
        {
            result = new FuzzyDate(year ?? 0, month ?? 0, day ?? 0);
            return true;
        }

        result = null;
        return false;
    }

    private static bool IsValid(int? year, int? month, int? day)
    {
        bool hasNoYear = !year.HasValue || year <= 0;
        bool hasNoMonth = !month.HasValue || month <= 0;
        bool hasNoDay = !day.HasValue || day <= 0;

        if (hasNoYear && hasNoMonth)
        {
            return false;
        }

        if (hasNoMonth && !hasNoDay)
        {
            return false;
        }

        return year > 0 && month <= 12 && day <= 31;
    }

    /// <summary>
    /// Creates a <see cref="FuzzyDate"/> object from a <see cref="DateTime"/> object.
    /// </summary>
    /// <param name="dateTime"></param>
    public static implicit operator FuzzyDate(DateTime dateTime)
    {
        return new FuzzyDate(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    /// <summary>
    /// Creates a <see cref="FuzzyDate"/> object from a <see cref="DateTimeOffset"/> object.
    /// </summary>
    /// <param name="dateTime"></param>
    public static implicit operator FuzzyDate(DateTimeOffset dateTime)
    {
        return new FuzzyDate(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    /// <summary>
    /// Indicates whether this instance and a specified object are equal.
    /// </summary>
    /// <param name="other">
    /// The object to compare with the current instance.
    /// </param>
    /// <returns>
    ///   <see langword="true" /> if <paramref name="other" /> and this instance are the same type and represent the same value; otherwise, <see langword="false" />.</returns>
    public override readonly bool Equals(object? other)
    {
        return other is FuzzyDate d && Equals(d);
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
    public readonly bool Equals(FuzzyDate other)
    {
        return (Year == other.Year || (Year == 0 && other.Year == 0)) &&
               (Month == other.Month || (Month == 0 && other.Month == 0)) &&
               (Day == other.Day || (Day == 0 && other.Day == 0));
    }

    /// <summary>
    /// Checks for equality.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static bool operator ==(FuzzyDate left, FuzzyDate right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks for inequality.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static bool operator !=(FuzzyDate left, FuzzyDate right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Checks whether <paramref name="left"/> is less than <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static bool operator <(FuzzyDate left, FuzzyDate right)
    {
        return left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Checks whether <paramref name="left"/> is less than or equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static bool operator <=(FuzzyDate left, FuzzyDate right)
    {
        return left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Checks whether <paramref name="left"/> is greater than <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static bool operator >(FuzzyDate left, FuzzyDate right)
    {
        return left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Checks whether <paramref name="left"/> is greater than or equal to <paramref name="right"/>.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static bool operator >=(FuzzyDate left, FuzzyDate right)
    {
        return left.CompareTo(right) >= 0;
    }

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
    ///  <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
    public readonly int CompareTo(FuzzyDate other)
    {
        if (Equals(other))
        {
            return 0;
        }

        int r = Year.CompareTo(other.Year);
        if (r != 0)
        {
            return r;
        }

        r = Month.CompareTo(other.Month);
        if (r != 0)
        {
            return r;
        }

        return Day.CompareTo(other.Day);
    }

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Year, Month, Day);
    }

    /// <summary>Returns the date in YYYY.MM.YY format, where the missing components are replaced with 0.</summary>
    /// <returns>The date in YYYY.MM.YY format, where the missing components are replaced with 0.</returns>
    public override readonly string ToString()
    {
        return $"{Year:D4}.{Month:D2}.{Day:D2}";
    }

    /// <summary>
    /// Returns this date as a human friendly string.
    /// </summary>
    /// <param name="abbreviate">Whether to abbreviate the month names.</param>
    public readonly string ToFriendlyString(bool abbreviate = false)
    {
        if (Year == 0)
        {
            return Day == 0
                ? (abbreviate ? CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(Month) : CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(Month))
                : $"{(abbreviate ? CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(Month) : CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(Month))} {GetDayWithSuffix(Day)}";
        }

        return Day == 0
            ? $"{(abbreviate ? CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(Month) : CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(Month))}, {Year}"
            : $"{(abbreviate ? CultureInfo.InvariantCulture.DateTimeFormat.GetAbbreviatedMonthName(Month) : CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(Month))} {GetDayWithSuffix(Day)}, {Year}";
    }

    private static string GetDayWithSuffix(int day)
    {
        return day switch
        {
            1 or 21 or 31 => $"{day}st",
            2 or 22 => $"{day}nd",
            3 or 23 => $"{day}rd",
            _ => $"{day}th",
        };
    }
}

/// <summary>
/// A <see cref="JsonConverter"/> for <see cref="FuzzyDate"/> objects.
/// </summary>
public class JsonDateConverter : JsonConverter<FuzzyDate>
{
    /// <inheritdoc />
    public override FuzzyDate Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var token = reader.TokenType;
        if (token == JsonTokenType.Number)
        {
            if (!reader.TryGetInt32(out int date))
            {
                throw new JsonException();
            }

            return FuzzyDate.Parse(date);
        }

        if (token == JsonTokenType.String)
        {
            string value = reader.GetString()!;
            return FuzzyDate.Parse(value);
        }

        throw new JsonException();
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, FuzzyDate value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
