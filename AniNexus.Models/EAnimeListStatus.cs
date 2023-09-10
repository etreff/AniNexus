namespace AniNexus;

/// <summary>
/// Defines the status of an anime in a user's list.
/// </summary>
public enum EAnimeListStatus
{
    /// <summary>
    /// The user plans on watching this anime.
    /// </summary>
    PlanToWatch = 1,

    /// <summary>
    /// The user is actively watching this anime.
    /// </summary>
    Watching = 2,

    /// <summary>
    /// The user has finished watching this anime to completion.
    /// </summary>
    Complete = 3,

    /// <summary>
    /// The user is rewatching this anime.
    /// </summary>
    Rewatching = 4,

    /// <summary>
    /// The user has stopped watching the anime but plans to start watching it again eventually.
    /// </summary>
    Paused = 5,

    /// <summary>
    /// The user has stopped watching the anime with no intention to watch it.
    /// </summary>
    Dropped = 6,

    /// <summary>
    /// The user has not started watching the series and does not plan to ever watch it.
    /// </summary>
    WillNeverWatch = 7
}
