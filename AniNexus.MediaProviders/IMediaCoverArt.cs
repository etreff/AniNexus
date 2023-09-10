namespace AniNexus.MediaProviders;

/// <summary>
/// Cover art information for a piece of media.
/// </summary>
public interface IMediaCoverArt
{
    /// <summary>
    /// The URL to a medium-sized cover art image.
    /// </summary>
    Uri? MediumUrl { get; }

    /// <summary>
    /// The URL to a large-sized cover art image.
    /// </summary>
    Uri? LargeUrl { get; }

    /// <summary>
    /// The cover art blob data.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This blob should contain JPG byte data.
    /// </para>
    /// <para>
    /// This property should not be set if it is possible to obtain an external URL.
    /// </para>
    /// </remarks>
    byte[]? Blob { get; }
}
