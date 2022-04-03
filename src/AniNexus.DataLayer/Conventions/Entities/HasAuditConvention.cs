using AniNexus.Data.Entities;
using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Data.Conventions.Entities;

/// <summary>
/// A convention that automatically configures entities that implement <see cref="IHasAudit"/>.
/// </summary>
internal sealed class HasAuditConvention : PreConfigureEntityConvention<IHasAudit>
{
    protected override bool AppliesToType(Type type)
    {
        return base.AppliesToType(type) && !type.IsTypeOf(typeof(AuditableEntity<,>));
    }

    protected override void OnPreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        var dateAddedProperty = entityType.FindProperty(nameof(IHasAudit.DateAdded))!;
        dateAddedProperty.SetComment("The date the entity was created.");
        dateAddedProperty.IsNullable = false;
        dateAddedProperty.SetComputedColumnSql("getutcdate()");
        dateAddedProperty.ValueGenerated = ValueGenerated.OnAdd;

        var dateUpdatedProperty = entityType.FindProperty(nameof(IHasAudit.DateUpdated))!;
        dateUpdatedProperty.SetComment("The date the entity was last updated.");
        dateUpdatedProperty.IsNullable = false;
        dateUpdatedProperty.SetComputedColumnSql("getutcdate()");
        dateUpdatedProperty.ValueGenerated = ValueGenerated.OnAddOrUpdate;
    }
}
