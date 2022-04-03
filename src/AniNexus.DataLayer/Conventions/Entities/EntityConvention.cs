using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Data.Conventions.Entities;

/// <summary>
/// The case class for an entity configuration convention.
/// </summary>
public abstract class EntityConvention<T> : IEntityConvention
{
    /// <summary>
    /// Defines whether the specified type is affected by this convention.
    /// </summary>
    /// <param name="type">The type.</param>
    protected virtual bool AppliesToType(Type type)
    {
        return type.IsTypeOf<T>();
    }
}

/// <summary>
/// The base class for an entity configuration convention that is applied before the model definitions
/// are manually defined.
/// </summary>
/// <typeparam name="T">The type for which this convention applies.</typeparam>
public abstract class PreConfigureEntityConvention<T> : EntityConvention<T>, IPreConfigureEntityConvention
{
    /// <inheritdoc/>
    public void PreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        if (entityType is null)
        {
            return;
        }

        var type = entityType.ClrType;
        if (!AppliesToType(type))
        {
            return;
        }

        OnPreConfigure(builder, entityType);
    }

    /// <summary>
    /// Configures the entity type.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    /// <param name="entityType">The entity type being configured.</param>
    protected abstract void OnPreConfigure(ModelBuilder builder, IMutableEntityType entityType);
}

/// <summary>
/// The base class for an entity configuration convention that is applied after the model definitions
/// are manually defined.
/// </summary>
/// <typeparam name="T">The type for which this convention applies.</typeparam>
public abstract class PostConfigureEntityConvention<T> : EntityConvention<T>, IPostConfigureEntityConvention
{
    /// <inheritdoc/>
    public void PostConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        if (entityType is null)
        {
            return;
        }

        var type = entityType.ClrType;
        if (!AppliesToType(type))
        {
            return;
        }

        OnPostConfigure(builder, entityType);
    }

    /// <summary>
    /// Configures the entity type.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    /// <param name="entityType">The entity type being configured.</param>
    protected abstract void OnPostConfigure(ModelBuilder builder, IMutableEntityType entityType);
}

/// <summary>
/// The base class for an entity configuration convention that is applied before and after the model definitions
/// are manually defined.
/// </summary>
/// <typeparam name="T">The type for which this convention applies.</typeparam>
public abstract class PrePostConfigureEntityConvention<T> : EntityConvention<T>, IPreConfigureEntityConvention, IPostConfigureEntityConvention
{
    /// <inheritdoc/>
    public void PreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        if (entityType is null)
        {
            return;
        }

        var type = entityType.ClrType;
        if (!AppliesToType(type))
        {
            return;
        }

        OnPreConfigure(builder, entityType);
    }

    /// <summary>
    /// Configures the entity type.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    /// <param name="entityType">The entity type being configured.</param>
    protected abstract void OnPreConfigure(ModelBuilder builder, IMutableEntityType entityType);

    /// <inheritdoc/>
    public void PostConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        if (entityType is null)
        {
            return;
        }

        var type = entityType.ClrType;
        if (!AppliesToType(type))
        {
            return;
        }

        OnPostConfigure(builder, entityType);
    }

    /// <summary>
    /// Configures the entity type.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    /// <param name="entityType">The entity type being configured.</param>
    protected abstract void OnPostConfigure(ModelBuilder builder, IMutableEntityType entityType);
}
