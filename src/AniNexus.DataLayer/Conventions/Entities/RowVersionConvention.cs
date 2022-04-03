using AniNexus.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Data.Conventions.Entities;

/// <summary>
/// A convention that configures entities that implement <see cref="IHasRowVersion"/>.
/// </summary>
internal sealed class RowVersionConvention : PreConfigureEntityConvention<IHasRowVersion>
{
    protected override void OnPreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        var rowVersionProperty = entityType.FindProperty(nameof(IHasRowVersion.RowVersion))!;
        rowVersionProperty.IsConcurrencyToken = true;
        rowVersionProperty.ValueGenerated = ValueGenerated.OnAddOrUpdate;
    }
}
