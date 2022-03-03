using System.ComponentModel.DataAnnotations;
using AniNexus.Data.Entities;
using AniNexus.Reflection;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Data.Validation;

/// <summary>
/// The context for validating properties.
/// </summary>
/// <typeparam name="TEntity">The entity type being validated.</typeparam>
/// <typeparam name="TProperty">The property type being validated.</typeparam>
public class PropertyValidatorBuilderContext<TEntity, TProperty>
    where TEntity : class, IEntity
{
    internal IList<ValidationResult> ValidationResults { get; } = new List<ValidationResult>();

    /// <summary>
    /// The property validator that this context was created for.
    /// </summary>
    public IPropertyValidatorBuilder<TEntity, TProperty> PropertyValidator { get; }

    /// <summary>
    /// The value being validated.
    /// </summary>
    public TProperty? Value { get; }

    /// <summary>
    /// The entity being validated.
    /// </summary>
    public TEntity Entity { get; }

    /// <summary>
    /// The full path of the property being validated.
    /// </summary>
    public string FullPropertyName { get; }

    /// <summary>
    /// The name of the property being validated.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Whether the actual property is nullable.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsNullableProperty { get; }

    private bool _addedNullCheck;

    /// <summary>
    /// Adds a validation result for this property.
    /// </summary>
    /// <param name="error">The validation error.</param>
    public void AddValidationResult(string error)
    {
        Guard.IsNotNullOrWhiteSpace(error, nameof(error));

        AddValidationResult(new ValidationResult(error, new[] { FullPropertyName }));
    }

    /// <summary>
    /// Adds a validation result for this property.
    /// </summary>
    /// <param name="validationResult">The validation result to add.</param>
    public void AddValidationResult(ValidationResult validationResult)
    {
        AddValidationResult(validationResult, false);
    }

    private void AddValidationResult(ValidationResult validationResult, bool isNullCheck)
    {
        if (validationResult is not null)
        {
            if (!isNullCheck || !_addedNullCheck)
            {
                ValidationResults.Add(validationResult);

                if (isNullCheck)
                {
                    _addedNullCheck = true;
                }
            }
        }
    }

    /// <summary>
    /// Adds a <see cref="ValidationResult"/> if <see cref="Value"/> is <see langword="null"/>
    /// and <see cref="IsNullableProperty"/> is <see langword="false"/>.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the validation result was added (the property is non-nullable and the value is <see langword="null"/>),
    /// <see langword="false"/> otherwise.</returns>
    public bool CheckNullability()
    {
        if (!IsNullableProperty && Value is null)
        {
            AddValidationResult(new ValidationResult("Value may not be null.", new[] { FullPropertyName }), true);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Adds a <see cref="ValidationResult"/> if <see cref="Value"/> is <see langword="null"/>.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the validation result was added (the value is <see langword="null"/>), <see langword="false"/> otherwise.</returns>
    public bool CheckForNull()
    {
        if (Value is null)
        {
            AddValidationResult(new ValidationResult("Value may not be null.", new[] { FullPropertyName }), true);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Adds a <see cref="ValidationResult"/> if <see cref="Value"/> is the default value for the type.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the validation result was added, <see langword="false"/> otherwise.</returns>
    [MemberNotNullWhen(false, nameof(Value))]
    public bool CheckForDefault()
    {
        if (Value.IsDefaultValue())
        {
            AddValidationResult(new ValidationResult($"Value may not be the default value for type {typeof(TProperty).Name}.", new[] { FullPropertyName }), true);
            return true;
        }

        return false;
    }

    internal PropertyValidatorBuilderContext(TEntity entity, TProperty? value, IPropertyValidatorBuilder<TEntity, TProperty> propertyValidator, string propertyName, bool isNullableProperty)
    {
        Value = value;
        Entity = entity;
        PropertyValidator = propertyValidator;
        FullPropertyName = propertyName;
        int lastIndex = FullPropertyName.LastIndexOf('.');
        PropertyName = lastIndex < 0
            ? FullPropertyName
            : FullPropertyName.Substring(lastIndex + 1);
        IsNullableProperty = isNullableProperty;
    }
}

/// <summary>
/// <see cref="PropertyValidatorBuilderContext{TEntity, TProperty}"/> extensions.
/// </summary>
public static partial class PropertyValidatorBuilderContextExtensions
{
    /// <summary>
    /// Creates a validator for every entity in a collection.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="context">The validator context.</param>
    /// <param name="childValidatorAction">The validation action to be applied to every entity in the collection.</param>
    public static PropertyValidatorBuilderContext<TEntity, IList<TProperty?>?> ForEach<TEntity, TProperty>(this PropertyValidatorBuilderContext<TEntity, IList<TProperty?>?> context, Action<ValidationBuilder<TProperty>, TProperty> childValidatorAction)
        where TEntity : class, IEntity
        where TProperty : class, IEntity
    {
        if (context.CheckNullability() || context.Value is null)
        {
            return context;
        }

        bool propertyTypeIsNullable = typeof(TProperty).IsNullable();
        for (int i = 0; i < context.Value.Count; ++i)
        {
            var entity = context.Value[i];
            if (entity is null)
            {
                if (!propertyTypeIsNullable)
                {
                    context.AddValidationResult("One or more elements is null.");
                }

                continue;
            }

            var validator = new ValidationBuilder<TProperty>(entity, context.FullPropertyName + $"[{i}]");
            childValidatorAction(validator, entity);
        }

        return context;
    }

    /// <summary>
    /// Creates a validator for every entity in a collection.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="context">The validator context.</param>
    /// <param name="childValidatorAction">The validation action to be applied to every entity in the collection.</param>
    public static PropertyValidatorBuilderContext<TEntity, IList<TProperty?>?> ForEach<TEntity, TProperty>(this PropertyValidatorBuilderContext<TEntity, IList<TProperty?>?> context, Action<ValidationBuilder, TProperty> childValidatorAction)
        where TEntity : class, IEntity
    {
        if (context.CheckNullability() || context.Value is null)
        {
            return context;
        }

        bool propertyTypeIsNullable = typeof(TProperty).IsNullable();
        foreach (var entity in context.Value)
        {
            if (entity is null)
            {
                if (!propertyTypeIsNullable)
                {
                    context.AddValidationResult("One or more elements is null.");
                }

                continue;
            }

            childValidatorAction(context.PropertyValidator.EntityValidator, entity);
        }

        return context;
    }

    /// <summary>
    /// Creates a validator for every entity in a collection.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="context">The validator context.</param>
    /// <param name="childValidatorAction">The validation action to be applied to every entity in the collection.</param>
    public static PropertyValidatorBuilderContext<TEntity, TProperty?[]?> ForEach<TEntity, TProperty>(this PropertyValidatorBuilderContext<TEntity, TProperty?[]?> context, Action<ValidationBuilder<TProperty>, TProperty> childValidatorAction)
        where TEntity : class, IEntity
        where TProperty : class, IEntity
    {
        if (context.CheckNullability() || context.Value is null)
        {
            return context;
        }

        bool propertyTypeIsNullable = typeof(TProperty).IsNullable();
        for (int i = 0; i < context.Value.Length; ++i)
        {
            var entity = context.Value[i];
            if (entity is null)
            {
                if (!propertyTypeIsNullable)
                {
                    context.AddValidationResult("One or more elements is null.");
                }

                continue;
            }

            var validator = new ValidationBuilder<TProperty>(entity, context.FullPropertyName + $"[{i}]");
            childValidatorAction(validator, entity);
        }

        return context;
    }

    /// <summary>
    /// Creates a validator for every entity in a collection.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    /// <param name="context">The validator context.</param>
    /// <param name="childValidatorAction">The validation action to be applied to every entity in the collection.</param>
    public static PropertyValidatorBuilderContext<TEntity, TProperty?[]?> ForEach<TEntity, TProperty>(this PropertyValidatorBuilderContext<TEntity, TProperty?[]?> context, Action<ValidationBuilder, TProperty> childValidatorAction)
        where TEntity : class, IEntity
    {
        if (context.CheckNullability() || context.Value is null)
        {
            return context;
        }

        bool propertyTypeIsNullable = typeof(TProperty).IsNullable();
        foreach (var entity in context.Value)
        {
            if (entity is null)
            {
                if (!propertyTypeIsNullable)
                {
                    context.AddValidationResult("One or more elements is null.");
                }

                continue;
            }

            childValidatorAction(context.PropertyValidator.EntityValidator, entity);
        }

        return context;
    }
}
