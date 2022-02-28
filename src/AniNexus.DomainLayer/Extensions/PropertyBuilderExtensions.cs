using AniNexus.Domain.Converters;

namespace AniNexus.Domain;

/// <summary>
/// <see cref="PropertyBuilder{TProperty}"/> extensions.
/// </summary>
public static class PropertyBuilderExtensions
{
    /// <summary>
    /// Configures a property of type <see cref="DateTime"/> to generate a UTC value on add.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="update">Whether to also generate a UTC value on update.</param>
    /// <returns>The property builder for chaining.</returns>
    public static PropertyBuilder<DateTime> HasComputedSqlDate(this PropertyBuilder<DateTime> builder, bool update = false)
    {
        builder.HasComputedColumnSql("getutcdate()");

        if (!update)
        {
            builder.ValueGeneratedOnAdd();
        }
        else
        {
            builder.ValueGeneratedOnAddOrUpdate();
        }

        return builder;
    }

    /// <summary>
    /// Configures the data type of the column that the property maps to when targeting
    /// a relational database to be a fixed with string.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="length">The string size in bytes.</param>
    /// <returns>The property builder for chaining.</returns>
    public static PropertyBuilder<string> IsFixedLength(this PropertyBuilder<string> builder, int length)
    {
        return builder.HasMaxLength(length).IsFixedLength();
    }

    /// <summary>
    /// Configures this property to use a converter that converts a list of string into a single string for storage.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="delimiter">The delimiter to use to separate elements in the collection.</param>
    /// <returns>The property builder for chaining.</returns>
    /// <remarks>
    /// If an element in the collection contains the specified delimiter, the result set will not returned properly. Ensure you choose a delimiter
    /// that will not be contained within the element collection.
    /// </remarks>
    public static PropertyBuilder<IList<string>> HasListConversion(this PropertyBuilder<IList<string>> builder, char delimiter = ListToStringConverter.DefaultDelimiter)
    {
        return builder.HasConversion(new ListToStringConverter(delimiter), ListToStringComparer.Instance);
    }

    /// <summary>
    /// Configures this property to use a converter that converts a list of elements of type <typeparamref name="T"/> into a single string for storage.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="valueConverter">A function to convert a string into an element of type <typeparamref name="T"/>.</param>
    /// <param name="delimiter">The delimiter to use to separate elements in the collection.</param>
    /// <returns>The property builder for chaining.</returns>
    public static PropertyBuilder<IList<T>> HasListConversion<T>(this PropertyBuilder<IList<T>> builder, Func<string, T> valueConverter, char delimiter = ListToStringConverter.DefaultDelimiter)
    {
        return HasListConversion(builder, valueConverter, delimiter);
    }
}
