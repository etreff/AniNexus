namespace AniNexus.Models;

/// <summary>
/// Age ratings for anime.
/// </summary>
public enum EAnimeAgeRating : byte
{
    [EnumMetadata("MinAge", 0)]
    [EnumMetadata("Abbreviation", "N/A")]
    [EnumMetadata("Description", "The age restriction is unknown.")]
    Unknown = 0,

    /// <summary>
    /// There is no age restriction of the anime.
    /// </summary>
    [EnumMetadata("MinAge", 0)]
    [EnumMetadata("Abbreviation", "G")]
    [EnumMetadata("Description", "There is no age restriction on the anime.")]
    Everyone = 1,

    /// <summary>
    /// The anime may have some mild violence and/or cursing.
    /// </summary>
    [EnumMetadata("MinAge", 10)]
    [EnumMetadata("Abbreviation", "PG")]
    [EnumMetadata("Description", "The anime may have some mild violence, suggestive humor, and/or cursing.")]
    Youth = 2,

    /// <summary>
    /// The anime may have some violence and/or pervese content.
    /// </summary>
    [EnumMetadata("MinAge", 13)]
    [EnumMetadata("Abbreviation", "PG-13")]
    [EnumMetadata("Description", "The anime may have some violence, nudity, and/or perverse content.")]
    Teen = 3,

    /// <summary>
    /// The anime has mild nudity or sexual situations, violent scenes, and/or strong language.
    /// </summary>
    [EnumMetadata("MinAge", 17)]
    [EnumMetadata("Abbreviation", "R-17+")]
    [EnumMetadata("Description", "The anime has mild nudity or sexual situations, violent scenes, and/or strong language.")]
    OlderTeen = 4,

    /// <summary>
    /// The anime has nudity, violent scenes, strong language, and/or mayhem,
    /// or it tackes a "string subject".
    /// </summary>
    [EnumMetadata("MinAge", 18)]
    [EnumMetadata("Abbreviation", "R-18+")]
    [EnumMetadata("Description", "The anime has nudity, violent scenes, strong language, and/or mayhem, or it tackles a \"strong subject\".")]
    Mature = 5,

    /// <summary>
    /// The anime has extreme sexual context, nudity, violent scenes, strong language, and/or mayhem,
    /// or it tackes a "string subject".
    /// </summary>
    [EnumMetadata("MinAge", 18)]
    [EnumMetadata("Abbreviation", "R-18+")]
    [EnumMetadata("Description", "The anime has extreme sexual content, nudity, violent scenes, strong language, and/or mayhem, or it tackles a \"strong subject\".")]
    Adult = 6
}
