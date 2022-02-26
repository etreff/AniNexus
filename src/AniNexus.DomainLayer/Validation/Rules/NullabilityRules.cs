using AniNexus.Domain.Entities;

namespace AniNexus.Domain.Validation;

public static partial class PropertyValidatorBuilderMethods
{
    /// <summary>
    /// Validates that a value is not <see langword="null"/> if the property is non-nullable.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsNotNull<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder)
        where TEntity : class, IEntity
        where TProperty : class
    {
        return builder.AddValidationRule(_ => { });
    }

    /// <summary>
    /// Validates that a value is not the default value of the type.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsNotDefault<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(false, context =>
        {
            context.CheckForDefault();
        });
    }
}
