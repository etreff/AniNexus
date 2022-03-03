using AniNexus.Data.Entities;
using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Data.Conventions;

/// <summary>
/// A convention that automatically configures entities that implement <see cref="IHasAudit"/>.
/// </summary>
internal sealed class HasAuditConvention : IPreConfigureEntityConvention
{
    public void PreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        if (entityType.ClrType.IsTypeOf<IHasAudit>() &&
            // AuditableEntity has this defined already. These are for auditable entities that
            // do not inherit the default auditable entity.
            !entityType.ClrType.IsTypeOf(typeof(AuditableEntity<,>)))
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
}
