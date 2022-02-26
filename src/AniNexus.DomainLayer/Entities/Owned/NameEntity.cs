using System.ComponentModel.DataAnnotations;
using AniNexus.Domain.Validation;
using Microsoft.Toolkit.Diagnostics;

namespace AniNexus.Domain.Entities;

/// <summary>
/// Models a name of an entity.
/// </summary>
public sealed class NameEntity : IOwnedEntity<NameEntity>
{
    /// <summary>
    /// The Id of the owner of this entity.
    /// </summary>
    public Guid OwnerId { get; set; }

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

    /// <summary>
    /// Whether the name is the primary name.
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <inheritdoc/>
    public void Validate(ValidationBuilder<NameEntity> validator)
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

/// <summary>
/// <see cref="OwnedNavigationBuilder"/> extensions.
/// </summary>
public static partial class OwnedNavigationBuilderExtensions
{
    /// <summary>
    /// Configures the <see cref="NameEntity"/> owned type for this entity.
    /// </summary>
    /// <typeparam name="TEntity">The owning type.</typeparam>
    /// <param name="builder">The owned type builder.</param>
    /// <param name="createTable">Whether to create table.</param>
    /// <param name="tableName">The table name to use. If left <see langword="null"/>, <see cref="Entity.GetDefaultTableName{TEntity}"/> will be used.</param>
    public static OwnedNavigationBuilder<TEntity, NameEntity> ConfigureNameEntity<TEntity>(this OwnedNavigationBuilder<TEntity, NameEntity> builder, bool createTable = true, string? tableName = null)
        where TEntity : class
    {
        Guard.IsNotNull(builder, nameof(builder));

        if (createTable)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableName = Entity.GetDefaultTableName<TEntity>();
            }
            builder.ToTable($"{tableName}Name");
        }

        builder.WithOwner().HasForeignKey(m => m.OwnerId);

        builder.Property(m => m.NativeName).HasComment("The native name.").HasColumnName(nameof(NameEntity.NativeName));
        builder.Property(m => m.RomajiName).HasComment("The romanization of the native name.").HasColumnName(nameof(NameEntity.RomajiName));
        builder.Property(m => m.EnglishName).HasComment("The name in English.").HasColumnName(nameof(NameEntity.EnglishName));
        builder.Property(m => m.IsPrimary).HasComment("Whether this name is the primary name of the series.").HasColumnName(nameof(NameEntity.IsPrimary));

        return builder;
    }
}
