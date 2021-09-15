namespace AniNexus.Models;

/// <summary>
/// Defines the status of an game in a user's list.
/// </summary>
public enum EGameListStatus : byte
{
    /// <summary>
    /// The user plans on playing this game.
    /// </summary>
    PlanToPlay = 1,

    /// <summary>
    /// The user is actively playing this game.
    /// </summary>
    Playing = 2,

    /// <summary>
    /// The user has finished playing this game to completion.
    /// </summary>
    Complete = 3,

    /// <summary>
    /// The user is replaying this game.
    /// </summary>
    Replaying = 4,

    /// <summary>
    /// The user has stopped playing the game but plans to start playing it again eventually.
    /// </summary>
    Paused = 5,

    /// <summary>
    /// The user has stopped playing the game with no intention to watch it.
    /// </summary>
    Dropped = 6,

    /// <summary>
    /// The user has not started playing the series and does not plan to ever play it.
    /// </summary>
    WillNeverPlay = 7
}
