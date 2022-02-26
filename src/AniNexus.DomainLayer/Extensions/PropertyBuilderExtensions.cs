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
}
