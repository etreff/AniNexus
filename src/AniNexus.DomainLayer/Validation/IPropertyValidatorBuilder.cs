using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using AniNexus.Domain.Entities;
using AniNexus.Reflection;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Domain.Validation;

internal interface IPropertyValidatorBuilder<TEntity>
    where TEntity : class, IEntity
{
    IEnumerable<ValidationResult> Validate(TEntity entity);
}

/// <summary>
/// A builder that configures property validations.
/// </summary>
/// <typeparam name="TEntity">The entity type being validated.</typeparam>
/// <typeparam name="TProperty">The property type being validated.</typeparam>
public interface IPropertyValidatorBuilder<TEntity, TProperty>
    where TEntity : class, IEntity
{
    /// <summary>
    /// The entity validator that this property validator is for.
    /// </summary>
    ValidationBuilder<TEntity> EntityValidator { get; }

    /// <summary>
    /// <para>
    /// Adds a validation rule to the property builder.
    /// </para>
    /// <para>
    /// If the property is <see langword="null"/> and the property is not nullable a validation result will be generated and the rule
    /// will not run.
    /// </para>
    /// <para>
    /// If the property is <see langword="null"/> and the property is nullable the method will not run.
    /// </para>
    /// </summary>
    /// <param name="rule">The validation rule.</param>
    /// <remarks>
    /// The context's property value is guaranteed to not be <see langword="null"/> in the context of the rule.
    /// </remarks>
    IPropertyValidatorBuilder<TEntity, TProperty> AddValidationRule(Action<PropertyValidatorBuilderContext<TEntity, TProperty>> rule)
        => AddValidationRule(true, rule);

    /// <summary>
    /// <para>
    /// Adds a validation rule to the property builder.
    /// </para>
    /// <para>
    /// If the property is <see langword="null"/> and the property is not nullable a validation result will be generated and the rule
    /// will not run.
    /// </para>
    /// <para>
    /// If the property is <see langword="null"/> and the property is nullable the method will not run.
    /// </para>
    /// </summary>
    /// <param name="nullCheck">Whether to null check before running the rule.</param>
    /// <param name="rule">The validation rule.</param>
    /// <remarks>
    /// The context's property value is guaranteed to not be <see langword="null"/> in the context of the rule if <paramref name="nullCheck"/> is <see langword="true"/>.
    /// </remarks>
    IPropertyValidatorBuilder<TEntity, TProperty> AddValidationRule(bool nullCheck, Action<PropertyValidatorBuilderContext<TEntity, TProperty>> rule);
}

internal class PropertyValidatorBuilder<TEntity, TProperty> : IPropertyValidatorBuilder<TEntity, TProperty>, IPropertyValidatorBuilder<TEntity>
    where TEntity : class, IEntity
{
    public ValidationBuilder<TEntity> EntityValidator { get; }

    private readonly List<(Action<PropertyValidatorBuilderContext<TEntity, TProperty>> Rule, bool NullCheck)> _validationRules = new();
    private readonly Expression<Func<TEntity, TProperty?>> _member;
    private readonly bool _isNullableProperty;
    private readonly string? _propertyPath;

    public PropertyValidatorBuilder(ValidationBuilder<TEntity> entityValidator, Expression<Func<TEntity, TProperty?>> member, string? propertyPath = null)
    {
        EntityValidator = entityValidator;
        _member = member;
        _isNullableProperty = _member.ReturnType.IsNullable();
        _propertyPath = propertyPath;
    }

    public IPropertyValidatorBuilder<TEntity, TProperty> AddValidationRule(bool nullCheck, Action<PropertyValidatorBuilderContext<TEntity, TProperty>> rule)
    {
        Guard.IsNotNull(rule, nameof(rule));

        _validationRules.Add((rule, nullCheck));

        return this;
    }

    public IEnumerable<ValidationResult> Validate(TEntity entity)
    {
        var value = _member.Compile().Invoke(entity);
        if (!_member.TryGetMemberName(out string? memberName))
        {
            throw new InvalidOperationException("Expression is not a member access expression.");
        }

        memberName = !string.IsNullOrWhiteSpace(_propertyPath)
            ? $"{_propertyPath}.{memberName}"
            : memberName;
        var context = new PropertyValidatorBuilderContext<TEntity, TProperty>(entity, value, this, memberName, _isNullableProperty);
        foreach (var validationRule in _validationRules)
        {
            if (!context.CheckNullability() && (!validationRule.NullCheck || context.Value is not null))
            {
                validationRule.Rule.Invoke(context);
            }
        }

        return context.ValidationResults;
    }
}
