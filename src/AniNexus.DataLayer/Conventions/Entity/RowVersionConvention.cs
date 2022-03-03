using AniNexus.Data.Entities;
using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Data.Conventions;

/// <summary>
/// A convention that configures entities that implement <see cref="IHasRowVersion"/>.
/// </summary>
internal sealed class RowVersionConvention : IPreConfigureEntityConvention
{
    public void PreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        if (entityType.ClrType.IsTypeOf<IHasRowVersion>())
        {
            var rowVersionProperty = entityType.FindProperty(nameof(IHasRowVersion.RowVersion))!;
            rowVersionProperty.IsConcurrencyToken = true;
            rowVersionProperty.ValueGenerated = ValueGenerated.OnAddOrUpdate;
        }
    }
}
