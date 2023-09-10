namespace AniNexus.MediaProviders.Anilist;

internal static class MediaScoreExtensions
{
    /// <summary>
    /// Returns the score list from the media score.
    /// </summary>
    /// <param name="score">The media scores.</param>
    public static double[] GetScoreList(this MediaScore score)
    {
        // WARNING: The API accepts an array of floats, not explicitly labeled floats. Be sure to keep this in sync with the actual order.
        // Note: The scores passed in are in a range from 0 to 100, so we need to divide by 10 since Anilist scores from 0 to 10.
        return new double[]
        {
            Math.Round(score.Story / 10.0, 1),
            Math.Round(score.Characters / 10.0, 1),
            Math.Round(score.Visuals / 10.0, 1),
            Math.Round(score.Audio / 10.0, 1),
            Math.Round(score.Enjoyment / 10.0, 1)
        };
    }
}
