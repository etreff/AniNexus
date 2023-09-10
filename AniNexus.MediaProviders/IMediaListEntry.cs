namespace AniNexus.MediaProviders;

/// <summary>
/// Defines a user's list entry for a specific piece of media.
/// </summary>
public interface IMediaListEntry
{
    /// <summary>
    /// The third-party Id of the media list entry.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// The third-party Id of the media.
    /// </summary>
    int MediaId { get; }

    /// <summary>
    /// The MyAnimeList Id of the media.
    /// </summary>
    /// <remarks>
    /// This may be the same as <see cref="MediaId"/>.
    /// </remarks>
    int? MalId { get; }

    /// <summary>
    /// The name of the media entry.
    /// </summary>
    string MediaName { get; }

    /// <summary>
    /// The format of the media entry.
    /// </summary>
    EMediaFormat MediaFormat { get; }

    /// <summary>
    /// The score out of 100 the user has given the media.
    /// </summary>
    decimal? Score { get; }

    /// <summary>
    /// The number of times the user has re-consumed the media.
    /// </summary>
    int Repeat { get; }

    /// <summary>
    /// The progress the user has made on this media.
    /// </summary>
    /// <remarks>
    /// Certain media like manga can have two different progress trackers - one for chapters and one for volumes.
    /// This value returns the smaller denomination. In the aforementioned case, the number of chapters is returned.
    /// </remarks>
    int Progress { get; }

    /// <summary>
    /// Notes the user has given for this media.
    /// </summary>
    string? Notes { get; }

    /// <summary>
    /// The status of the media in the user's list.
    /// </summary>
    EMediaListStatus Status { get; }

    /// <summary>
    /// The date at which the user started consuming this series.
    /// </summary>
    DateOnly? StartedAt { get; }

    /// <summary>
    /// The date at which the user finished consuming this series.
    /// </summary>
    DateOnly? CompletedAt { get; }
}
