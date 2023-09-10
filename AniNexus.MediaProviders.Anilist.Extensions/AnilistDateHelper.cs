namespace AniNexus.MediaProviders.Anilist;

/// <summary>
/// A utility for working with Anilist's date values.
/// </summary>
public static class AnilistDateHelper
{
    private static readonly DateTimeOffset Epoch = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

    /// <summary>
    /// Transforms the number received from Anilist about an air date into a <see cref="DateTimeOffset"/> object.
    /// </summary>
    /// <param name="airingAt">The number received from Anilist.</param>
    public static DateTime GetAiringAtDateTime(int airingAt)
    {
        return Epoch.AddSeconds(airingAt).UtcDateTime;
    }

    /// <summary>
    /// Transforms the number received from Anilist about the time until an episode airs into a <see cref="TimeSpan"/> object.
    /// </summary>
    /// <param name="timeUntilAiring">The number received from Anilist.</param>
    public static TimeSpan GetTimeUntilAiring(int timeUntilAiring)
    {
        return TimeSpan.FromSeconds(timeUntilAiring);
    }

    /// <summary>
    /// Adds a time to the current time and converts that number into a value that Anilist can understand.
    /// </summary>
    /// <param name="time">The time to add to the current time.</param>
    public static int AddTimeFromNow(TimeSpan time)
    {
        var diff = (DateTimeOffset.Now - Epoch).Duration();
        return (int)diff.Add(time).TotalSeconds;
    }
}
