using System.ComponentModel.DataAnnotations;
using AniNexus.Data.Validation;

namespace AniNexus.Data.Entities;

/// <summary>
/// Models a name of an entity.
/// </summary>
public sealed class NameEntity : OwnedEntity<NameEntity>
{
    /// <summary>
    /// The name in the native language.
    /// </summary>
    public string? NativeName { get; set; }

    /// <summary>
    /// The romanization of the native name.
    /// </summary>
    public string? RomajiName { get; set; }

    /// <summary>
    /// The name in English.
    /// </summary>
    public string? EnglishName { get; set; }

    /// <inheritdoc/>
    protected override string GetTableName(Type ownerType, string ownerEntityTableName)
    {
        // Generally we will have an entity with a primary name and aliases. The primary name
        // will be stored inline with the entity and have a list of name entities as aliases.
        // We will account for the more common case here, and any models that deviate from this
        // pattern will need to explicitly specify the table name during configuration.
        return $"{ownerEntityTableName}NameAlias";
    }

    /// <inheritdoc/>
    protected override void ConfigureEntity<TOwnerEntity>(OwnedNavigationBuilder<TOwnerEntity, NameEntity> builder)
    {
        builder.Property(m => m.NativeName).HasComment("The native name.").HasColumnName(nameof(NativeName));
        builder.Property(m => m.RomajiName).HasComment("The romanization of the native name.").HasColumnName(nameof(RomajiName));
        builder.Property(m => m.EnglishName).HasComment("The name in English.").HasColumnName(nameof(EnglishName));
    }

    /// <inheritdoc/>
    public override void Validate(ValidationBuilder<NameEntity> validator)
    {
        validator.AddValidationRule((b, e) =>
        {
            if (string.IsNullOrWhiteSpace(e.NativeName) && string.IsNullOrWhiteSpace(e.RomajiName) && string.IsNullOrWhiteSpace(e.EnglishName))
            {
                b.AddValidationResult(new ValidationResult("One or more names does not have at least one name must be set.", b.GetPropertyPathArray(nameof(NativeName))));
                return;
            }

            if (e.NativeName != null && string.IsNullOrWhiteSpace(e.NativeName))
            {
                b.AddValidationResult(new ValidationResult("Value cannot be an empty string or whitespace.", b.GetPropertyPathArray(nameof(NativeName))));
            }

            if (e.RomajiName != null && string.IsNullOrWhiteSpace(e.RomajiName))
            {
                b.AddValidationResult(new ValidationResult("Value cannot be an empty string or whitespace.", b.GetPropertyPathArray(nameof(RomajiName))));
            }

            if (e.EnglishName != null && string.IsNullOrWhiteSpace(e.EnglishName))
            {
                b.AddValidationResult(new ValidationResult("Value cannot be an empty string or whitespace.", b.GetPropertyPathArray(nameof(EnglishName))));
            }
        });
    }
}
