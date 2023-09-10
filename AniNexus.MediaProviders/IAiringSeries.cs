namespace AniNexus.MediaProviders;

/// <summary>
/// Defines a series that is airing this season.
/// </summary>
public interface IAiringSeries
{
    /// <summary>
    /// The third-party Id of the series.
    /// </summary>
    int MediaId { get; }

    /// <summary>
    /// The episode number that is airing next.
    /// </summary>
    decimal Episode { get; }

    /// <summary>
    /// The UTC date and time at which the episode will air.
    /// </summary>
    DateTime AiringAt { get; }

    /// <summary>
    /// The time until which the episode airs.
    /// </summary>
    TimeSpan TimeUntilAirs { get; }

    /// <summary>
    /// The metadata of the series that is airing.
    /// </summary>
    IMediaMetadataLite Metadata { get; }

    /// <summary>
    /// The user's media list entry for this series.
    /// </summary>
    IMediaListEntry? MediaListEntry { get; }
}
