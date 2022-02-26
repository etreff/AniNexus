namespace AniNexus.Domain.Entities;

/// <summary>
/// Declares than an entity is a translation entity for another entity type.
/// </summary>
/// <typeparam name="TForType">The type that the translations are for.</typeparam>
public interface IIsTranslation<TForType>
    where TForType : IEntity
{
    /// <summary>
    /// The Id of the reference of this translation.
    /// </summary>
    Guid ReferenceId { get; set; }

    /// <summary>
    /// The Id of the language this translation is for.
    /// </summary>
    short LanguageId { get; set; }

    /// <summary>
    /// The media genre translation.
    /// </summary>
    string Translation { get; set; }

    /// <summary>
    /// The reference that this translation is for.
    /// </summary>
    TForType Reference { get; set; }

    /// <summary>
    /// The language this translation is for.
    /// </summary>
    LanguageEntity Language { get; set; }
}
