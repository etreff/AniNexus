namespace AniNexus.Data.Entities;

/// <summary>
/// When added to a class, the model will have a nagivation property to translations for this entity.
/// </summary>
public interface IHasTranslations<TTranslationEntity, TReferenceEntity, TReferenceEntityKey>
    where TTranslationEntity : TranslationEntity<TTranslationEntity, TReferenceEntity, TReferenceEntityKey>
    where TReferenceEntity : Entity<TReferenceEntity, TReferenceEntityKey>, IHasTranslations<TTranslationEntity, TReferenceEntity, TReferenceEntityKey>
    where TReferenceEntityKey : struct, IComparable<TReferenceEntityKey>, IEquatable<TReferenceEntityKey>
{
    /// <summary>
    /// The translations for this entity.
    /// </summary>
    IList<TTranslationEntity> Translations { get; set; }
}

/// <summary>
/// When added to a class, the model will have a nagivation property to translations for this entity.
/// </summary>
public interface IHasTranslations<TTranslationEntity, TReferenceEntity> : IHasTranslations<TTranslationEntity, TReferenceEntity, Guid>
    where TTranslationEntity : TranslationEntity<TTranslationEntity, TReferenceEntity>
    where TReferenceEntity : Entity<TReferenceEntity>, IHasTranslations<TTranslationEntity, TReferenceEntity>
{
}
