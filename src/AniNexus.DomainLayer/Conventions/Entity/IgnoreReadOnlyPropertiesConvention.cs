using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Domain.Conventions;

/// <summary>
/// A convention that automatically ignores read-only properties on the entity.
/// </summary>
internal sealed class IgnoreReadOnlyPropertiesConvention : IPreConfigureEntityConvention
{
    public void PreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        // Find get-only properties and exclude them from navigation property discovery by convention.
        // If we do not do this the navigation builder will complain about multiple relationships.
        // There is likely some convention around get-only properties, but we do not use that convention
        // here. If a model wants to explicitly create a property over a get-only property they are
        // free to do so.
        foreach (var property in entityType.ClrType.GetProperties().Where(static p => !p.CanWrite))
        {
            entityType.AddIgnored(property.Name);
        }

        foreach (var entityProperty in entityType.GetProperties())
        {
            // Configure nullability and ignored properties.
            if (entityProperty.PropertyInfo is not null)
            {
                // Do a double check. The model creator seems to only pick up writeable properties,
                // but that behavior may not be the case in the future.
                bool isReadOnlyProperty = entityProperty.PropertyInfo.GetSetMethod() is null;
                if (isReadOnlyProperty)
                {
                    entityType.AddIgnored(entityProperty.PropertyInfo.Name);
                }
                else
                {
                    entityProperty.IsNullable = entityProperty.PropertyInfo.IsNullable();
                }
            }
        }
    }
}
