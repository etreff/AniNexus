namespace AniNexus.Models;

/// <summary>
/// The category of a game.
/// </summary>
public enum EGameCategory : byte
{
    /// <summary>
    /// The media is a video game that is not a visual novel.
    /// </summary>
    Digital = 1,

    /// <summary>
    /// The media is a video game that is a visual novel.
    /// </summary>
    VisualNovel = 2,

    /// <summary>
    /// The media is a game that is not a video game.
    /// </summary>
    Analog = 3,

    /// <summary>
    /// The media falls under a category that is not defined.
    /// </summary>
    Other = 99
}
