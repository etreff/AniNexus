using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using AniNexus.Domain.Entities;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Domain.Validation;

/// <summary>
/// Builds validation errors.
/// </summary>
public abstract class ValidationBuilder
{
    private readonly IList<ValidationResult> _validationResults = new List<ValidationResult>();

    private protected ValidationBuilder()
    {
    }

    /// <summary>
    /// Directly adds a validation result to the <see cref="ValidationBuilder{TEntity}"/>.
    /// </summary>
    /// <param name="result">The result to add.</param>
    /// <returns>This builder for chaining.</returns>
    public virtual ValidationBuilder AddValidationResult(ValidationResult result)
    {
        Guard.IsNotNull(result, nameof(result));

        _validationResults.Add(result);

        return this;
    }

    internal IEnumerable<ValidationResult> Validate()
    {
        return ValidateImpl(new ReadOnlyCollection<ValidationResult>(_validationResults));
    }

    // Not sure if this is necessary with a private protected constructor.
    private protected abstract IEnumerable<ValidationResult> ValidateImpl(IReadOnlyList<ValidationResult> validationResults);
}

/// <summary>
/// Builds validation errors.
/// </summary>
/// <typeparam name="TEntity">The entity that this builder is building validations for.</typeparam>
public sealed class ValidationBuilder<TEntity> : ValidationBuilder
    where TEntity : class, IEntity
{
    private readonly IList<Action<ValidationBuilder<TEntity>, TEntity>> _entityValidationRules = new List<Action<ValidationBuilder<TEntity>, TEntity>>();
    private readonly IList<IPropertyValidatorBuilder<TEntity>> _propertyValidators = new List<IPropertyValidatorBuilder<TEntity>>();
    private readonly TEntity _entity;
    private readonly string? _propertyPath;

    internal ValidationBuilder(TEntity entity, string? propertyPath)
    {
        _entity = entity;
        _propertyPath = propertyPath;
    }

    /// <summary>
    /// Gets the full property path using the specified property as the final slug in the name.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    public string GetPropertyPath(string propertyName)
    {
        Guard.IsNotNullOrWhiteSpace(propertyName, nameof(propertyName));

        return string.IsNullOrWhiteSpace(_propertyPath)
            ? propertyName
            : $"{_propertyPath}.{propertyName}";
    }

    /// <summary>
    /// Gets the full property path using the specified property as the final slug in the name.
    /// </summary>
    /// <param name="propertyName">The property name.</param>
    public string[] GetPropertyPathArray(string propertyName)
    {
        return new[] { GetPropertyPath(propertyName) };
    }

    /// <summary>
    /// Returns a validator for the specified property.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="member">The member to get the validator for.</param>
    public IPropertyValidatorBuilder<TEntity, TProperty?> Property<TProperty>(Expression<Func<TEntity, TProperty?>> member)
    {
        Guard.IsNotNull(member, nameof(member));

        var builder = new PropertyValidatorBuilder<TEntity, TProperty?>(this, member, _propertyPath);
        _propertyValidators.Add(builder);

        return builder;
    }

    /// <summary>
    /// Validates a <see cref="IOwnedEntity{T}"/>.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="member">The member to get the validator for.</param>
    public IPropertyValidatorBuilder<TEntity, TProperty?> ValidateOwnedEntity<TProperty>(Expression<Func<TEntity, TProperty?>> member)
        where TProperty : class, IOwnedEntity<TProperty>
    {
        var propertyValidator = Property(member);
        propertyValidator.ValidateOwnedEntity();

        return propertyValidator;
    }

    /// <summary>
    /// Validates a collection of <see cref="IOwnedEntity{T}"/>.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="member">The member to get the validator for.</param>
    public IPropertyValidatorBuilder<TEntity, IList<TProperty?>?> ValidateOwnedEntities<TProperty>(Expression<Func<TEntity, IList<TProperty?>?>> member)
        where TProperty : class, IOwnedEntity<TProperty>
    {
        var propertyValidator = Property(member);
        propertyValidator.ValidateOwnedEntities();

        return propertyValidator;
    }

    /// <summary>
    /// Validates a collection of <see cref="IOwnedEntity{T}"/>.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="member">The member to get the validator for.</param>
    public IPropertyValidatorBuilder<TEntity, TProperty?[]?> ValidateOwnedEntities<TProperty>(Expression<Func<TEntity, TProperty?[]?>> member)
        where TProperty : class, IOwnedEntity<TProperty>
    {
        var propertyValidator = Property(member);
        propertyValidator.ValidateOwnedEntities();

        return propertyValidator;
    }

    /// <summary>
    /// Directly adds a validation result to the <see cref="ValidationBuilder{TEntity}"/>.
    /// </summary>
    /// <param name="result">The result to add.</param>
    /// <returns>This builder for chaining.</returns>
    public override ValidationBuilder<TEntity> AddValidationResult(ValidationResult result)
    {
        base.AddValidationResult(result);

        return this;
    }

    /// <summary>
    /// Adds a validation rule that expands one or more properties.
    /// </summary>
    /// <param name="validationRule">An action that receives the entity being validated.</param>
    /// <returns>This builder for chaining.</returns>
    public ValidationBuilder<TEntity> AddValidationRule(Action<ValidationBuilder<TEntity>, TEntity> validationRule)
    {
        _entityValidationRules.Add(validationRule);

        return this;
    }

    private protected override IEnumerable<ValidationResult> ValidateImpl(IReadOnlyList<ValidationResult> validationResults)
    {
        var result = Enumerable.Empty<ValidationResult>();

        // Well formed property validators from Validator.Property(m => name).
        foreach (var validator in _propertyValidators)
        {
            result = result.Concat(validator.Validate(_entity));
        }

        // Entity-level validation rules from Validator.AddValidationRule().
        foreach (var validator in _entityValidationRules)
        {
            var entityValidator = new ValidationBuilder<TEntity>(_entity, _propertyPath);
            validator(entityValidator, _entity);

            result = result.Concat(entityValidator.Validate());
        }

        // Loose validation results that were explicitly added to the validation builder
        // from Validator.AddValidationResult().
        result = result.Concat(validationResults);

        return result;
    }
}

/// <summary>
/// <see cref="ValidationBuilder{TEntity}"/> extensions.
/// </summary>
public static partial class ValidationBuilderExtensions
{
}
