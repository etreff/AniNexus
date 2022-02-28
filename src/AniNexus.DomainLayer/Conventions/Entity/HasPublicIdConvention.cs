using AniNexus.Domain.Entities;
using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AniNexus.Domain.Conventions;

/// <summary>
/// A convention that automatically configures entities that implement <see cref="IHasPublicId{T}"/>.
/// </summary>
internal sealed class HasPublicIdConvention : IPreConfigureEntityConvention
{
    public void PreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        if (entityType.ClrType.IsTypeOf(typeof(IHasPublicId<>)))
        {
            var publicIdProperty = entityType.FindProperty(nameof(IHasPublicId.PublicId))!;
            publicIdProperty.SetComment("The public key of this entity used for public APIs and navigation URLs.");
            publicIdProperty.IsNullable = false;

            entityType.AddIndex(publicIdProperty).IsUnique = true;

            var keyType = entityType.ClrType.GetInterfaces().Single(static i => i.IsTypeOf(typeof(IHasPublicId<>))).GenericTypeArguments[0];
            if (keyType == typeof(Guid))
            {
                publicIdProperty.ValueGenerated = ValueGenerated.Never;
                publicIdProperty.SetValueGeneratorFactory(static (_, _) => new SequentialGuidValueGenerator());
            }
            else if (keyType.IsIntegerType())
            {
                publicIdProperty.ValueGenerated = ValueGenerated.OnAdd;

                // We don't need the real table name here - we just need a distinct value, and the default entity
                // table name is distinct per class.
                string sequenceId = $"{Entity.GetDefaultTableName(entityType.ClrType)}.{nameof(IHasPublicId.PublicId)}";
                publicIdProperty.SetDefaultValueSql($"NEXT VALUE FOR {sequenceId}");
                builder.HasSequence(sequenceId).StartsAt(1).IncrementsBy(1);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported {nameof(IHasPublicId.PublicId)} key type '{keyType.FullName}'.");
            }
        }
    }
}
