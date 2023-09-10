namespace AniNexus;

/// <summary>
/// The category of an anime.
/// </summary>
public enum EAnimeCategory
{
    /// <summary>
    /// The media aired on TV.
    /// </summary>
    TV = 1,

    /// <summary>
    /// The media was made specifically for release in home-video formats.
    /// </summary>
    OVA = 2,

    /// <summary>
    /// The media was released directly on the internet.
    /// </summary>
    ONA = 3,

    /// <summary>
    /// The media is a single episode movie.
    /// </summary>
    Movie = 4,

    /// <summary>
    /// The media is a special that was included in a DVD/Blu-ray release, a picture drama,
    /// or a pilot.
    /// </summary>
    Special = 5,

    /// <summary>
    /// The media is a video featuring live actors.
    /// </summary>
    LiveAction = 6,

    /// <summary>
    /// The media is a music video.
    /// </summary>
    Music = 7,

    /// <summary>
    /// The media falls under a category that is not defined.
    /// </summary>
    Other = 99
}
