using AniNexus.Collections.Concurrent;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AniNexus.Domain.Converters;

/// <summary>
/// Extensions for <see cref="PropertyBuilder"/> that focus on converters.
/// </summary>
public static class PropertyBuilderConversionExtensions
{
    private static readonly ThreadSafeCache<string, object[]> _converters = new(static delimiter => new object[]
   {
        new DelimiterValueConverter(delimiter),
        new DelimiterValueComparer(delimiter)
   });

    /// <summary>
    /// Adds a converter that transforms a <see cref="IList{T}"/> of <see cref="string"/> to and from a single
    /// <see cref="string"/> using a pipe character '|' as a delimiter between elements.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <returns>The property builder for chaining.</returns>
    public static PropertyBuilder<IList<string>?> HasDelimitedConversion(this PropertyBuilder<IList<string>?> builder)
        => HasDelimitedConversion(builder, "|");

    /// <summary>
    /// Adds a converter that transforms a <see cref="IList{T}"/> of <see cref="string"/> to and from a single
    /// <see cref="string"/> using the value specified in <paramref name="delimiter"/> as a delimiter between elements.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <returns>The property builder for chaining.</returns>
    public static PropertyBuilder<IList<string>?> HasDelimitedConversion(this PropertyBuilder<IList<string>?> builder, string delimiter)
    {
        object[] objects = _converters.Get(delimiter);

        return builder.HasConversion((ValueConverter)objects[0], (ValueComparer)objects[1]);
    }

    private class DelimiterValueConverter : ValueConverter<IList<string>?, string?>
    {
        public DelimiterValueConverter(string delimiter)
            : base(
                v => v != null ? string.Join(delimiter, v) : null,
                static v => v != null ? v.Split('|', StringSplitOptions.RemoveEmptyEntries) : null)
        {
        }
    }

    private class DelimiterValueComparer : ValueComparer<IList<string>?>
    {
        public DelimiterValueComparer(string delimiter)
            : base(
                static (a, b) => (a != null && b != null) ? a.SequenceEqual(b) : true,
                a => a != null ? string.Join(delimiter, a).GetHashCode() : 0)
        {
        }
    }
}
