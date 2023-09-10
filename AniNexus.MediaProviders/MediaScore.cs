namespace AniNexus.MediaProviders;

/// <summary>
/// Models the score categories for a piece of media.
/// </summary>
/// <param name="Story">The score for the story, between 0 and 100.</param>
/// <param name="Characters">The score for the characters, between 0 and 100.</param>
/// <param name="Visuals">The score for the visual and animation quality, between 0 and 100.</param>
/// <param name="Audio">The score for the audio quality, between 0 and 100.</param>
/// <param name="Enjoyment">The score for the overall enjoyment, between 0 and 100.</param>
public record struct MediaScore(int Story, int Characters, int Visuals, int Audio, int Enjoyment)
{
    /// <summary>
    /// The overall score that is the average of the other scores.
    /// </summary>
    public int Overall
    {
        get
        {
            int score = 0;
            int count = 0;

            if (Story != 0)
            {
                score += Story;
                ++count;
            }
            if (Characters != 0)
            {
                score += Characters;
                ++count;
            }
            if (Visuals != 0)
            {
                score += Visuals;
                ++count;
            }
            if (Audio != 0)
            {
                score += Audio;
                ++count;
            }
            if (Enjoyment != 0)
            {
                score += Enjoyment;
                ++count;
            }

            if (count > 0)
            {
                return score / count;
            }
            else
            {
                return 0;
            }
        }
    }
}
