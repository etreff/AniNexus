using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Data.Conventions;

/// <summary>
/// A convention that configures defaults for primitive types.
/// </summary>
internal sealed class PrimitiveDefaultValueConvention : IPostConfigureEntityConvention
{
    public void PostConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        foreach (var entityProperty in entityType.GetProperties())
        {
            // Nullable properties will be assigned NULL by default. Only non-nullable
            // properties are the issue.
            if (entityProperty.ClrType.IsNullable())
            {
                continue;
            }

            // The default value was already set.
            if (entityProperty.GetDefaultValue() is not null)
            {
                continue;
            }

            // Set default values for booleans.
            if (entityProperty.ClrType == typeof(bool))
            {
                entityProperty.SetDefaultValue(false);
            }

            // Set default values for numbers.
            if (entityProperty.ClrType.IsNumeric())
            {
                // We do not want to set default values (constants) for
                // important keys or indices.
                bool isPartOfIndexOrKey = entityProperty.IsKey() || entityProperty.IsIndex();
                if (!isPartOfIndexOrKey)
                {
                    entityProperty.SetDefaultValue(0);
                }
            }
        }
    }
}
