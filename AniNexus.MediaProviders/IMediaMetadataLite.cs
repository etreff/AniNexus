namespace AniNexus.MediaProviders;

/// <summary>
/// Defines minimal metadata for a piece of media.
/// </summary>
public interface IMediaMetadataLite
{
    /// <summary>
    /// The third-party Id of the media.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// The name of the piece of media.
    /// </summary>
    IMediaName Name { get; }

    /// <summary>
    /// The season the media was released.
    /// </summary>
    /// <remarks>
    /// This is only relevant if <see cref="Format"/> is an animation format.
    /// </remarks>
    EMediaSeason? Season { get; }

    /// <summary>
    /// The total number of episodes or chapters the media has.
    /// </summary>
    int? TotalParts { get; }

    /// <summary>
    /// The format of the media.
    /// </summary>
    EMediaFormat Format { get; }

    /// <summary>
    /// The status of the media.
    /// </summary>
    EMediaStatus Status { get; }

    /// <summary>
    /// The URL to the third-party web view of this piece of media.
    /// </summary>
    Uri ProviderUrl { get; }

    /// <summary>
    /// The URL to this media's homepage.
    /// </summary>
    Uri? OfficialUrl { get; }

    /// <summary>
    /// The cover art for this media.
    /// </summary>
    IMediaCoverArt CoverArt { get; }

    /// <summary>
    /// The date at which the media ended.
    /// </summary>
    FuzzyDate? EndDate { get; }

    /// <summary>
    /// The year in which the media started.
    /// </summary>
    int? Year { get; }

    /// <summary>
    /// The UTC date and time at which this data was fetched.
    /// </summary>
    DateTime Timestamp { get; }
}
