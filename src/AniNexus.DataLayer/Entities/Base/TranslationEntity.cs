using System.ComponentModel.DataAnnotations;
using AniNexus.Data.Validation;

namespace AniNexus.Data.Entities;

/// <summary>
/// The base class of a translation model.
/// </summary>
/// <typeparam name="TTranslationEntity">The type of this entity.</typeparam>
/// <typeparam name="TReferenceEntity">The type that the translations are for.</typeparam>
/// <typeparam name="TReferenceEntityKey">The type of the primary key of <typeparamref name="TReferenceEntity"/>.</typeparam>
public abstract class TranslationEntity<TTranslationEntity, TReferenceEntity, TReferenceEntityKey> : Entity<TTranslationEntity, TReferenceEntityKey>, IIsTranslation<TReferenceEntity>
    where TTranslationEntity : TranslationEntity<TTranslationEntity, TReferenceEntity, TReferenceEntityKey>
    where TReferenceEntity : Entity<TReferenceEntity, TReferenceEntityKey>, IHasTranslations<TTranslationEntity, TReferenceEntity, TReferenceEntityKey>
    where TReferenceEntityKey : struct, IComparable<TReferenceEntityKey>, IEquatable<TReferenceEntityKey>
{
    /// <summary>
    /// The Id of the reference of this translation.
    /// </summary>
    public Guid ReferenceId { get; set; }

    /// <summary>
    /// The Id of the language this translation is for.
    /// </summary>
    public short LanguageId { get; set; }

    /// <summary>
    /// The translation.
    /// </summary>
    public string Translation { get; set; } = default!;

    /// <summary>
    /// The reference that this translation is for.
    /// </summary>
    public TReferenceEntity Reference { get; set; } = default!;

    /// <summary>
    /// The language this translation is for.
    /// </summary>
    public LanguageEntity Language { get; set; } = default!;

    /// <inheritdoc/>
    protected override void ConfigureEntity(EntityTypeBuilder<TTranslationEntity> builder)
    {
        // 1. Primary key specification (if not Entity<>)
        // 2. Index specification
        builder.HasIndex(m => new { m.ReferenceId, m.LanguageId }).IsUnique();
        builder.HasIndex(m => m.LanguageId);
        // 3. Navigation properties
        builder.HasOne(m => m.Reference).WithMany(m => m.Translations).HasForeignKey(m => m.ReferenceId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        // 4. Propery specification
        builder.Property(m => m.Translation).IsUnicode().HasComment("The translation.");
    }

    /// <inheritdoc/>
    protected override void Validate(ValidationContext validationContext, ValidationBuilder<TTranslationEntity> validator)
    {
        base.Validate(validationContext, validator);

        validator.Property(m => m.Translation).IsNotNullOrWhiteSpace();
    }

    /// <inheritdoc/>
    protected override string GetTableName()
    {
        return $"{Entity.GetDefaultTableName<TReferenceEntity>()}_i18n";
    }
}

/// <summary>
/// The base class of a translation model.
/// </summary>
/// <typeparam name="TTranslationEntity">The type of this entity.</typeparam>
/// <typeparam name="TReferenceEntity">The type that the translations are for.</typeparam>
public abstract class TranslationEntity<TTranslationEntity, TReferenceEntity> : TranslationEntity<TTranslationEntity, TReferenceEntity, Guid>
    where TTranslationEntity : TranslationEntity<TTranslationEntity, TReferenceEntity>
    where TReferenceEntity : Entity<TReferenceEntity>, IHasTranslations<TTranslationEntity, TReferenceEntity>
{
}
