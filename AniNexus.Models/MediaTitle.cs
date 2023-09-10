namespace AniNexus;

/// <summary>
/// The various titles of a piece of media.
/// </summary>
public class MediaTitle
{
    /// <summary>
    /// The name of the media in the title's native language.
    /// </summary>
    public string NativeName { get; set; } = default!;

    /// <summary>
    /// The Romanization of the title's native name.
    /// </summary>
    public string? RomajiName { get; set; }

    /// <summary>
    /// The name of the media in English.
    /// </summary>
    public string? EnglishName { get; set; }
}
