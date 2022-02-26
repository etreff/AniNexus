using AniNexus.Domain.Entities;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Domain.Validation;

/// <summary>
/// <see cref="IPropertyValidatorBuilder{TEntity, TProperty}"/> methods.
/// </summary>
public static partial class PropertyValidatorBuilderMethods
{
    /// <summary>
    /// <para>
    /// Validates that a value is not null and less than to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is less than a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsLessThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(context =>
        {
            if (context.Value!.CompareTo(value) >= 0)
            {
                context.AddValidationResult(message ?? $"Value must be less than {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and less than to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is less than a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsLessThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(context =>
        {
            var compareToValue = value(context.Entity);
            if (context.Value!.CompareTo(compareToValue) >= 0)
            {
                context.AddValidationResult(message ?? $"Value must be less than {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and less than to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is less than a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsLessThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Value.CompareTo(value) >= 0)
            {
                context.AddValidationResult(message ?? $"Value must be less than {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and less than to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is less than a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsLessThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(context =>
        {
            var compareToValue = value(context.Entity);
            if (context.Value!.Value.CompareTo(compareToValue) >= 0)
            {
                context.AddValidationResult(message ?? $"Value must be less than {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and less than or equal to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is less than or equal to a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsLessThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(context =>
        {
            if (context.Value!.CompareTo(value) > 0)
            {
                context.AddValidationResult(message ?? $"Value must be less than or equal to {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and less than or equal to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is less than or equal to a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsLessThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(context =>
        {
            var compareToValue = value(context.Entity);
            if (context.Value!.CompareTo(compareToValue) > 0)
            {
                context.AddValidationResult(message ?? $"Value must be less than or equal to {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and less than or equal to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is less than or equal to a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsLessThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Value.CompareTo(value) > 0)
            {
                context.AddValidationResult(message ?? $"Value must be less than or equal to {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and less than or equal to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is less than or equal to a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsLessThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(context =>
        {
            var compareToValue = value(context.Entity);
            if (context.Value!.Value.CompareTo(compareToValue) > 0)
            {
                context.AddValidationResult(message ?? $"Value must be less than or equal to {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and greater than to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is greater than a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsGreaterThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(context =>
        {
            if (context.Value!.CompareTo(value) <= 0)
            {
                context.AddValidationResult(message ?? $"Value must be greater than {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and greater than to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is greater than a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsGreaterThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(context =>
        {
            var compareToValue = value(context.Entity);
            if (context.Value!.CompareTo(compareToValue) <= 0)
            {
                context.AddValidationResult(message ?? $"Value must be greater than {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and greater than to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is greater than a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsGreaterThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Value.CompareTo(value) <= 0)
            {
                context.AddValidationResult(message ?? $"Value must be greater than {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and greater than to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is greater than a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsGreaterThan<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(context =>
        {
            var compareToValue = value(context.Entity);
            if (context.Value!.Value.CompareTo(compareToValue) <= 0)
            {
                context.AddValidationResult(message ?? $"Value must be greater than {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and greater than or equal to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is greater than or equal to a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsGreaterThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(context =>
        {
            if (context.Value!.CompareTo(value) < 0)
            {
                context.AddValidationResult(message ?? $"Value must be greater than or equal to {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and greater than or equal to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is greater than or equal to a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsGreaterThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(context =>
        {
            var compareToValue = value(context.Entity);
            if (context.Value!.CompareTo(compareToValue) < 0)
            {
                context.AddValidationResult(message ?? $"Value must be greater than or equal to {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and greater than or equal to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is greater than or equal to a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsGreaterThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(context =>
        {
            if (context.Value!.Value.CompareTo(value) < 0)
            {
                context.AddValidationResult(message ?? $"Value must be greater than or equal to {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and greater than or equal to a certain value if the
    /// property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is greater than or equal to a certain value if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsGreaterThanOrEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(context =>
        {
            var compareToValue = value(context.Entity);
            if (context.Value!.Value.CompareTo(compareToValue) < 0)
            {
                context.AddValidationResult(message ?? $"Value must be greater than or equal to {value}.");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and is between two values if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is between two values if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="min">The lower value to compare to.</param>
    /// <param name="max">The lower value to compare to.</param>
    /// <param name="minInclusivity">The inclusivity of the lower boundary.</param>
    /// <param name="maxInclusivity">The inclusivity of the upper boundary.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="min"/> is <see langword="null"/> -or-
    /// <paramref name="max"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsBetween<TEntity, TProperty>(
        this IPropertyValidatorBuilder<TEntity, TProperty?> builder,
        TProperty min,
        TProperty max,
        EInclusivity minInclusivity = EInclusivity.Inclusive,
        EInclusivity maxInclusivity = EInclusivity.Inclusive,
        string? message = null)
        where TEntity : class, IEntity
        where TProperty : IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(context =>
        {
            if (!context.Value!.IsBetween(min, max, minInclusivity, maxInclusivity))
            {
                context.AddValidationResult(message ?? $"Value must be between {min} ({minInclusivity}) and {max} ({maxInclusivity}).");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and is between two values if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is between two values if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="min">The lower value to compare to.</param>
    /// <param name="max">The lower value to compare to.</param>
    /// <param name="minInclusivity">The inclusivity of the lower boundary.</param>
    /// <param name="maxInclusivity">The inclusivity of the upper boundary.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="min"/> is <see langword="null"/> -or-
    /// <paramref name="max"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsBetween<TEntity, TProperty>(
        this IPropertyValidatorBuilder<TEntity, TProperty?> builder,
        Func<TEntity, TProperty> min,
        Func<TEntity, TProperty> max,
        EInclusivity minInclusivity = EInclusivity.Inclusive,
        EInclusivity maxInclusivity = EInclusivity.Inclusive,
        string? message = null)
        where TEntity : class, IEntity
        where TProperty : IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(min, nameof(min));
        Guard.IsNotNull(max, nameof(max));

        return builder.AddValidationRule(context =>
        {
            if (!context.Value!.IsBetween(min(context.Entity), max(context.Entity), minInclusivity, maxInclusivity))
            {
                context.AddValidationResult(message ?? $"Value must be between {min} ({minInclusivity}) and {max} ({maxInclusivity}).");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and is between two values if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is between two values if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="min">The lower value to compare to.</param>
    /// <param name="max">The lower value to compare to.</param>
    /// <param name="minInclusivity">The inclusivity of the lower boundary.</param>
    /// <param name="maxInclusivity">The inclusivity of the upper boundary.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="min"/> is <see langword="null"/> -or-
    /// <paramref name="max"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsBetween<TEntity, TProperty>(
        this IPropertyValidatorBuilder<TEntity, TProperty?> builder,
        TProperty? min,
        TProperty? max,
        EInclusivity minInclusivity = EInclusivity.Inclusive,
        EInclusivity maxInclusivity = EInclusivity.Inclusive,
        string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(min, nameof(min));
        Guard.IsNotNull(max, nameof(max));

        return builder.AddValidationRule(context =>
        {
            if (!context.Value!.Value.IsBetween(min.Value, max.Value, minInclusivity, maxInclusivity))
            {
                context.AddValidationResult(message ?? $"Value must be between {min} ({minInclusivity}) and {max} ({maxInclusivity}).");
            }
        });
    }

    /// <summary>
    /// <para>
    /// Validates that a value is not null and is between two values if the property is non-nullable.
    /// </para>
    /// <para>
    /// Validates that a value is between two values if the property is nullable but set.
    /// </para>
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="min">The lower value to compare to.</param>
    /// <param name="max">The lower value to compare to.</param>
    /// <param name="minInclusivity">The inclusivity of the lower boundary.</param>
    /// <param name="maxInclusivity">The inclusivity of the upper boundary.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="min"/> is <see langword="null"/> -or-
    /// <paramref name="max"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsBetween<TEntity, TProperty>(
        this IPropertyValidatorBuilder<TEntity, TProperty?> builder,
        Func<TEntity, TProperty> min,
        Func<TEntity, TProperty> max,
        EInclusivity minInclusivity = EInclusivity.Inclusive,
        EInclusivity maxInclusivity = EInclusivity.Inclusive,
        string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IComparable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(min, nameof(min));
        Guard.IsNotNull(max, nameof(max));

        return builder.AddValidationRule(context =>
        {
            if (!context.Value!.Value.IsBetween(min(context.Entity), max(context.Entity), minInclusivity, maxInclusivity))
            {
                context.AddValidationResult(message ?? $"Value must be between {min} ({minInclusivity}) and {max} ({maxInclusivity}).");
            }
        });
    }

    /// <summary>
    /// Validates that a value is equal to a certain value.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/></exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IEquatable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(false, context =>
        {
            if (context.Value is null && value is null)
            {
                return;
            }

            if (!context.Value!.Equals(value))
            {
                context.AddValidationResult(message ?? $"Value must be equal to {value}.");
            }
        });
    }

    /// <summary>
    /// Validates that a value is equal to a certain value.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IEquatable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(false, context =>
        {
            if (context.Value is null && value is null)
            {
                return;
            }

            if (!context.Value!.Equals(value(context.Entity)))
            {
                context.AddValidationResult(message ?? $"Value must be equal to {value}.");
            }
        });
    }

    /// <summary>
    /// Validates that a value is equal to a certain value.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty? value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IEquatable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(false, context =>
        {
            if (context.Value is null && value is null)
            {
                return;
            }

            if (!context.Value!.Equals(value))
            {
                context.AddValidationResult(message ?? $"Value must be equal to {value}.");
            }
        });
    }

    /// <summary>
    /// Validates that a value is equal to a certain value.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty?> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IEquatable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(false, context =>
        {
            if (context.Value is null && value is null)
            {
                return;
            }

            if (!context.Value!.Equals(value(context.Entity)))
            {
                context.AddValidationResult(message ?? $"Value must be equal to {value}.");
            }
        });
    }

    /// <summary>
    /// Validates that a value is not equal to a certain value.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsNotEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty? value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IEquatable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(false, context =>
        {
            if (context.Value is null && value is null)
            {
                return;
            }

            if (context.Value!.Equals(value))
            {
                context.AddValidationResult(message ?? $"Value must not be equal to {value}.");
            }
        });
    }

    /// <summary>
    /// Validates that a value is not equal to a certain value.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsNotEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty?> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : IEquatable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(false, context =>
        {
            if (context.Value is null && value is null)
            {
                return;
            }

            if (context.Value!.Equals(value(context.Entity)))
            {
                context.AddValidationResult(message ?? $"Value must not be equal to {value}.");
            }
        });
    }

    /// <summary>
    /// Validates that a value is not equal to a certain value.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsNotEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, TProperty? value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IEquatable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));

        return builder.AddValidationRule(false, context =>
        {
            if (context.Value is null && value is null)
            {
                return;
            }

            if (context.Value!.Value.Equals(value))
            {
                context.AddValidationResult(message ?? $"Value must not be equal to {value}.");
            }
        });
    }

    /// <summary>
    /// Validates that a value is not equal to a certain value.
    /// </summary>
    /// <param name="builder">The property builder.</param>
    /// <param name="value">The value to compare to.</param>
    /// <param name="message">An optional error message to override the default error message.</param>
    /// <returns>This builder for chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <see langword="null"/> -or-
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static IPropertyValidatorBuilder<TEntity, TProperty?> IsNotEqualTo<TEntity, TProperty>(this IPropertyValidatorBuilder<TEntity, TProperty?> builder, Func<TEntity, TProperty?> value, string? message = null)
        where TEntity : class, IEntity
        where TProperty : struct, IEquatable<TProperty>
    {
        Guard.IsNotNull(builder, nameof(builder));
        Guard.IsNotNull(value, nameof(value));

        return builder.AddValidationRule(false, context =>
        {
            if (context.Value is null && value is null)
            {
                return;
            }

            if (context.Value!.Value.Equals(value(context.Entity)))
            {
                context.AddValidationResult(message ?? $"Value must not be equal to {value}.");
            }
        });
    }
}
