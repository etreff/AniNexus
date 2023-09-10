namespace AniNexus.MediaProviders;

/// <summary>
/// A studio that worked on a piece of media.
/// </summary>
public interface IMediaStudio
{
    /// <summary>
    /// The international name of the studio.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Whether this studio was involved in the animation/drawing of the media.
    /// </summary>
    /// <remarks>
    /// If this is set to <see langword="false"/>, the studio acted as a producer.
    /// </remarks>
    bool IsAnimationStudio { get; }

    /// <summary>
    /// The URL to the third-party web view of this studio.
    /// </summary>
    Uri? ProviderUrl { get; }

    /// <summary>
    /// The URL to this studio's homepage.
    /// </summary>
    Uri? OfficialUrl { get; }
}
