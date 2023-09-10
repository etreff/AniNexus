namespace AniNexus;

/// <summary>
/// Defines the status of an manga in a user's list.
/// </summary>
public enum EMangaListStatus
{
    /// <summary>
    /// The user plans on reading this manga.
    /// </summary>
    PlanToRead = 1,

    /// <summary>
    /// The user is actively reading this manga.
    /// </summary>
    Reading = 2,

    /// <summary>
    /// The user has finished reading this manga to completion.
    /// </summary>
    Complete = 3,

    /// <summary>
    /// The user is rereading this manga.
    /// </summary>
    Rereading = 4,

    /// <summary>
    /// The user has stopped reading the manga but plans to start reading it again eventually.
    /// </summary>
    Paused = 5,

    /// <summary>
    /// The user has stopped reading the manga with no intention to watch it.
    /// </summary>
    Dropped = 6,

    /// <summary>
    /// The user has not started reading the series and does not plan to ever read it.
    /// </summary>
    WillNeverRead = 7
}
