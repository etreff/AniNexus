namespace AniNexus.MediaProviders;

/// <summary>
/// Defines the status of a media list entry.
/// </summary>
public enum EMediaListStatus
{
    /// <summary>
    /// Currently watching/reading
    /// </summary>
    Current,

    /// <summary>
    /// Planning to watch/read
    /// </summary>
    Planning,

    /// <summary>
    /// Finished watching/reading
    /// </summary>
    Completed,

    /// <summary>
    /// Stopped watching/reading before completing
    /// </summary>
    Dropped,

    /// <summary>
    /// Paused watching/reading
    /// </summary>
    Paused,

    /// <summary>
    /// Re-watching/reading
    /// </summary>
    Repeating
}
