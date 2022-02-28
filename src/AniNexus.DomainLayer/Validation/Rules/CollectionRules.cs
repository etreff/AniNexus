using AniNexus.Domain.Entities;
using AniNexus.Reflection;

namespace AniNexus.Domain.Validation;

public static partial class PropertyValidatorBuilderMethods
{
    /// <summary>
    /// Applies rules to all elements of a collection-type property.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being validated.</typeparam>
    /// <typeparam name="TProperty">The property type being validated.</typeparam>
    /// <param name="builder">The property builder.</param>
    /// <param name="elementValidator">The validator for the element in the collection</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> ForEach<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> builder, Action<ValidationBuilder<TProperty>> elementValidator)
        where TEntity : class, IEntity
        where TProperty : class, IEntity
    {
        return builder.AddValidationRule(context => context.ForEach((v, _) => elementValidator(v)));
    }

    /// <summary>
    /// Applies rules to all elements of a collection-type property.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being validated.</typeparam>
    /// <typeparam name="TProperty">The property type being validated.</typeparam>
    /// <param name="builder">The property builder.</param>
    /// <param name="elementValidator">The validator for the element in the collection</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> ForEach<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> builder, Action<ValidationBuilder, TProperty> elementValidator)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context => context.ForEach((v, p) => elementValidator(v, p)));
    }

    /// <summary>
    /// Applies rules to all elements of a collection-type property.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being validated.</typeparam>
    /// <typeparam name="TProperty">The property type being validated.</typeparam>
    /// <param name="builder">The property builder.</param>
    /// <param name="elementValidator">The validator for the element in the collection</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, TProperty?[]?> ForEach<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?[]?> builder, Action<ValidationBuilder<TProperty>> elementValidator)
        where TEntity : class, IEntity
        where TProperty : class, IEntity
    {
        return builder.AddValidationRule(context => context.ForEach((v, _) => elementValidator(v)));
    }

    /// <summary>
    /// Applies rules to all elements of a collection-type property.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being validated.</typeparam>
    /// <typeparam name="TProperty">The property type being validated.</typeparam>
    /// <param name="builder">The property builder.</param>
    /// <param name="elementValidator">The validator for the element in the collection</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, TProperty?[]?> ForEach<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?[]?> builder, Action<ValidationBuilder, TProperty> elementValidator)
        where TEntity : class, IEntity
        where TProperty : class, IEntity
    {
        return builder.AddValidationRule(context => context.ForEach((v, p) => elementValidator(v, p)));
    }

    /// <summary>
    /// <para>
    /// Validates that a collection has less than <paramref name="count"/> elements if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a collection has less than <paramref name="count"/> elements  if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="count">The maximum exclusive length of the collection.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> HasCountLessThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> builder, int count)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Count >= count)
            {
                context.AddValidationResult($"Collection must have less than {count} elements.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a collection has less than <paramref name="count"/> elements if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a collection has less than <paramref name="count"/> elements  if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="count">The maximum exclusive length of the collection.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, TProperty?[]?> HasCountLessThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?[]?> builder, int count)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Length >= count)
            {
                context.AddValidationResult($"Collection must have less than {count} elements.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a collection has less than or equal to <paramref name="count"/> elements if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a collection has less than or equal to <paramref name="count"/> elements  if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="count">The maximum inclusive length of the collection.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> HasCountLessThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> builder, int count)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Count > count)
            {
                context.AddValidationResult($"Collection must have less than {count} elements.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a collection has less than or equal to <paramref name="count"/> elements if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a collection has less than or equal to <paramref name="count"/> elements  if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="count">The maximum inclusive length of the collection.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, TProperty?[]?> HasCountLessThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?[]?> builder, int count)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Length > count)
            {
                context.AddValidationResult($"Collection must have less than {count} elements.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a collection has greater than <paramref name="count"/> elements if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a collection has greater than <paramref name="count"/> elements  if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="count">The minimum exclusive length of the collection.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> HasCountGreaterThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> builder, int count)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Count <= count)
            {
                context.AddValidationResult($"Collection must have greater than {count} elements.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a collection has greater than <paramref name="count"/> elements if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a collection has greater than <paramref name="count"/> elements  if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="count">The minimum exclusive length of the collection.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, TProperty?[]?> HasCountGreaterThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?[]?> builder, int count)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Length <= count)
            {
                context.AddValidationResult($"Collection must have greater than {count} elements.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a collection has greater than or equal to <paramref name="count"/> elements if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a collection has greater than or equal to <paramref name="count"/> elements  if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="count">The minimum inclusive length of the collection.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> HasCountGreaterThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> builder, int count)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Count < count)
            {
                context.AddValidationResult($"Collection must have greater than {count} elements.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a collection has greater than or equal to <paramref name="count"/> elements if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a collection has greater than or equal to <paramref name="count"/> elements  if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="count">The minimum inclusive length of the collection.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, TProperty?[]?> HasCountGreaterThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?[]?> builder, int count)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Length < count)
            {
                context.AddValidationResult($"Collection must have greater than {count} elements.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a collection and its contents are not <see langword="null"/> or empty if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a collection and its contents are not empty if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> IsNotEmpty<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> builder)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Count == 0)
            {
                context.AddValidationResult("Value may not be empty.");
            }

            if (typeof(TProperty).IsNullable())
            {
                return;
            }

            foreach (var element in context.Value)
            {
                if (element is null)
                {
                    context.AddValidationResult("One or more elements in the collection is null.");
                    break;
                }
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a collection and its contents are not <see langword="null"/> or empty if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a collection and its contents are not empty if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, TProperty?[]?> IsNotEmpty<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?[]?> builder)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Length == 0)
            {
                context.AddValidationResult("Value may not be empty.");
            }

            if (typeof(TProperty).IsNullable())
            {
                return;
            }

            foreach (var element in context.Value)
            {
                if (element is null)
                {
                    context.AddValidationResult("One or more elements in the collection is null.");
                    break;
                }
            }
        });
    }

    /// <summary>
    /// Validates that exactly one element in the collection satisfied a certain condition.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being validated.</typeparam>
    /// <typeparam name="TProperty">The property type being validated.</typeparam>
    /// <param name="builder">The property builder.</param>
    /// <param name="predicate">The predicate.</param>
    /// <param name="message">An error message.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> Single<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> builder, Predicate<TProperty?> predicate, string? message = null)
        where TEntity : class, IEntity
        where TProperty : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            bool flag = false;
            foreach (var entity in context.Value!)
            {
                bool triggeredCondition = predicate(entity);
                if (flag && triggeredCondition)
                {
                    context.AddValidationResult(message ?? "More than one element matches the predicate.");
                    return;
                }
                flag = triggeredCondition;
            }

            if (!flag)
            {
                context.AddValidationResult(message ?? "No elements match the predicate.");
            }
        });
    }

    /// <summary>
    /// Validates that exactly one element in the collection satisfied a certain condition.
    /// </summary>
    /// <typeparam name="TEntity">The entity type being validated.</typeparam>
    /// <typeparam name="TProperty">The property type being validated.</typeparam>
    /// <param name="builder">The property builder.</param>
    /// <param name="predicate">The predicate.</param>
    /// <param name="message">An error message.</param>
    /// <returns>This builder for chaining.</returns>
    public static IPropertyValidatorBuilder<TEntity, TProperty?[]?> Single<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?[]?> builder, Predicate<TProperty?> predicate, string? message = null)
        where TEntity : class, IEntity
    {
        return builder.AddValidationRule(context =>
        {
            bool flag = false;
            foreach (var entity in context.Value!)
            {
                bool triggeredCondition = predicate(entity);
                if (flag && triggeredCondition)
                {
                    context.AddValidationResult(message ?? "More than one element matches the predicate.");
                    return;
                }
                flag = triggeredCondition;
            }

            if (!flag)
            {
                context.AddValidationResult(message ?? "No elements match the predicate.");
            }
        });
    }
}
