using AniNexus.Domain.Entities;

namespace AniNexus.Domain.Validation;

public static partial class PropertyValidatorBuilderMethods
{
    /// <summary>
    /// <para>
    /// Validates that a value is not <see langword="null"/>, empty, or whitespace if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is not empty or whitespace if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, string?> IsNotNullOrWhiteSpace<TEntity>(this IPropertyValidatorBuilder<TEntity, string?> builder)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (string.IsNullOrWhiteSpace(context.Value))
            {
                context.AddValidationResult("Value may not be null, empty, or whitespace.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not <see langword="null"/> and a valid URL if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is a valid URL if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, string?> IsValidUrl<TEntity>(this IPropertyValidatorBuilder<TEntity, string?> builder)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (!Uri.TryCreate(context.Value, UriKind.Absolute, out _))
            {
                context.AddValidationResult("Value is not a valid, absolute URL.");
            }
        });
    }
}
