namespace AniNexus.MediaProviders;

/// <summary>
/// A tag for a piece of media that acts as a descriptor.
/// </summary>
public interface IMediaTag
{
    /// <summary>
    /// The short name of the tag.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// A longer description of the tag.
    /// </summary>
    string? Description { get; }

    /// <summary>
    /// The rank of the tag. The higher the rank, the more relevant the tag is.
    /// </summary>
    int Rank { get; }

    /// <summary>
    /// Whether this tag indicates that the media may contain adult themes.
    /// </summary>
    bool IsAdult { get; }
}
