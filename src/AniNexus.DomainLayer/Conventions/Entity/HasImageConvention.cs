using AniNexus.Domain.Entities;
using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Domain.Conventions;

/// <summary>
/// A convention that automatically configures entities that implement <see cref="IHasImage"/>.
/// </summary>
internal sealed class HasImageConvention : IPreConfigureEntityConvention
{
    public void PreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        if (entityType.ClrType.IsTypeOf<IHasImage>())
        {
            var isSoftDeleteProperty = entityType.FindProperty(nameof(IHasImage.ImageId))!;
            isSoftDeleteProperty.SetComment("The Id of the image for this entity.");
            isSoftDeleteProperty.IsNullable = true;
            isSoftDeleteProperty.SetDefaultValue(null);
        }
    }
}
