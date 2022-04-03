using AniNexus.Data.Entities;

namespace AniNexus.Data.Validation;

public static partial class PropertyValidatorBuilderMethods
{
    /// <summary>
    /// Validates that a value is not an empty <see cref="Guid"/>.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, Guid> IsNotEmpty<TEntity>(this IPropertyValidatorBuilder<TEntity, Guid> builder)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(false, context =>
        {
            if (context.Value == Guid.Empty)
            {
                context.AddValidationResult("Value may not be an empty Guid.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not <see langword="null"/> or an empty <see cref="Guid"/> if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is not an empty <see cref="Guid"/> if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, Guid?> IsNotEmpty<TEntity>(this IPropertyValidatorBuilder<TEntity, Guid?> builder)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Value == Guid.Empty)
            {
                context.AddValidationResult("Value may not be an empty Guid.");
            }
        });
    }
}
