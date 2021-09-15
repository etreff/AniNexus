namespace AniNexus.Models;

/// <summary>
/// Defines how one media entry relates to another.
/// </summary>
public enum EMediaRelationType : byte
{
    /// <summary>
    /// The entry is a prequel of a specific media entry.
    /// </summary>
    Prequel = 1,

    /// <summary>
    /// The entry is a sequel of a specific media entry.
    /// </summary>
    Sequel = 2,

    /// <summary>
    /// The entry is a side story a specific media entry.
    /// </summary>
    SideStory = 3,

    /// <summary>
    /// The entry is a parent story of a specific media entry.
    /// </summary>
    ParentStory = 4,

    /// <summary>
    /// The entry is a recap of a specific media entry.
    /// </summary>
    Recap = 5,

    /// <summary>
    /// The entry takes place in an alternate setting of a specific media entry.
    /// </summary>
    AlternativeSetting = 6,

    /// <summary>
    /// The entry is a retelling of a specific media entry.
    /// </summary>
    AlternativeVersion = 7,

    /// <summary>
    /// The entry is a music video of a specific media entry.
    /// </summary>
    MusicVideo = 8,

    /// <summary>
    /// The entry is downloadable content of a specific media entry.
    /// </summary>
    DLC = 9,

    /// <summary>
    /// The entry is related in a way that is not covered by any other relation type.
    /// </summary>
    Other = 99
}
