namespace AniNexus.Models;

/// <summary>
/// Age ratings for manga.
/// </summary>
public enum EMangaAgeRating
{
    /// <summary>
    /// There is no age restriction of the manga.
    /// </summary>
    [EnumMetadata("MinAge", 0)]
    [EnumMetadata("Abbreviation", "E")]
    [EnumMetadata("Description", "There is no age restriction on the manga.")]
    Everyone = 1,

    /// <summary>
    /// The manga may have some mild violence and/or cursing.
    /// </summary>
    [EnumMetadata("MinAge", 10)]
    [EnumMetadata("Abbreviation", "Y")]
    [EnumMetadata("Description", "The manga may have some mild violence and/or cursing.")]
    Youth = 2,

    /// <summary>
    /// The manga may have some violence and/or pervese content.
    /// </summary>
    [EnumMetadata("MinAge", 13)]
    [EnumMetadata("Abbreviation", "T")]
    [EnumMetadata("Description", "The manga may have some violence and/or perverse content.")]
    Teen = 3,

    /// <summary>
    /// The manga has sexual situations, violent scenes, and/or strong language.
    /// </summary>
    [EnumMetadata("MinAge", 16)]
    [EnumMetadata("Abbreviation", "OT")]
    [EnumMetadata("Description", "The manga has sexual situations, violent scenes, and/or strong language.")]
    OlderTeen = 4,

    /// <summary>
    /// The manga has sexual situations, violent scenes, strong language, and/or mayhem,
    /// or it tackes a "string subject".
    /// </summary>
    [EnumMetadata("MinAge", 18)]
    [EnumMetadata("Abbreviation", "M")]
    [EnumMetadata("Description", "The manga has sexual situations, violent scenes, strong language, and/or mayhem, or it tackles a \"strong subject\".")]
    Mature = 5
}
