using AniNexus.Data.Entities;
using AniNexus.Reflection;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AniNexus.Data.Conventions;

/// <summary>
/// A convention that automatically configures entities that implement <see cref="IIsTranslation{TForType}"/>.
/// </summary>
internal sealed class IsTranslationConvention : IPreConfigureEntityConvention
{
    public void PreConfigure(ModelBuilder builder, IMutableEntityType entityType)
    {
        if (entityType.ClrType.IsTypeOf(typeof(IIsTranslation<>)) &&
            // TranslationEntity has this defined already. These are for translation entities that
            // do not inherit the default translation entity.
            !entityType.ClrType.IsTypeOf(typeof(TranslationEntity<,>)))
        {
            var referenceType = entityType.ClrType.GetInterfaces().First(t => t.IsTypeOf(typeof(IIsTranslation<>))).GenericTypeArguments[0];
            var entityBuilder = builder.Entity(entityType.ClrType);

            entityBuilder
                .HasOne(referenceType, nameof(IIsTranslation<object>.Reference))
                .WithOne()
                .HasForeignKey(entityType.ClrType, nameof(IIsTranslation<object>.ReferenceId))
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            entityBuilder
                .HasOne(referenceType, nameof(IIsTranslation<object>.Language))
                .WithMany()
                .HasForeignKey(nameof(IIsTranslation<object>.LanguageId))
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            var translationProperty = entityType.FindProperty(nameof(IIsTranslation<object>.Translation))!;
            translationProperty.SetComment("The translation.");
            translationProperty.IsNullable = false;
            translationProperty.SetColumnType("nvarchar(1000)");
        }
    }
}
