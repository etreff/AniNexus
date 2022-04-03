using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Data.Conventions;

/// <summary>
/// Defines an entity configuration convention.
/// </summary>
/// <remarks>
/// Do not inherit this interface directly as it will not be picked up by the
/// convention system. Inherit from <see cref="IPreConfigureEntityConvention"/> and/or
/// <see cref="IPostConfigureEntityConvention"/> instead.
/// </remarks>
public interface IEntityConvention
{
}

/// <summary>
/// Defines an entity configuration convention that is applied before the model definitions
/// are manually defined.
/// </summary>
public interface IPreConfigureEntityConvention : IEntityConvention
{
    /// <summary>
    /// Configures the entity type.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    /// <param name="entityType">The entity type being configured.</param>
    void PreConfigure(ModelBuilder builder, IMutableEntityType entityType);
}

/// <summary>
/// Defines an entity configuration convention that is applied after the model definitions
/// are manually defined.
/// </summary>
public interface IPostConfigureEntityConvention : IEntityConvention
{
    /// <summary>
    /// Configures the entity type.
    /// </summary>
    /// <param name="builder">The model builder.</param>
    /// <param name="entityType">The entity type being configured.</param>
    void PostConfigure(ModelBuilder builder, IMutableEntityType entityType);
}
