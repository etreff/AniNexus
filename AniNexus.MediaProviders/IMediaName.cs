namespace AniNexus.MediaProviders;

/// <summary>
/// The name of a piece of media.
/// </summary>
public interface IMediaName
{
    /// <summary>
    /// The native name of the piece of media.
    /// </summary>
    string NativeName { get; }

    /// <summary>
    /// The English name of the piece of media.
    /// </summary>
    string? EnglishName { get; }

    /// <summary>
    /// The Romanization of <see cref="NativeName"/>.
    /// </summary>
    string? RomajiName { get; }
}
