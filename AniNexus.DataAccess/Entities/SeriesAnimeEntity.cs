namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Models an anime series.
/// </summary>
public sealed class SeriesAnimeEntity : SeriesMediaEntity
{
    /// <summary>
    /// The Id of the season the anime will be released in.
    /// </summary>
    /// <remarks>
    /// The season refers to the Japanese anime schedule.
    /// </remarks>
    /// <seealso cref="EAnimeSeason"/>
    /// <seealso cref="AnimeSeasonTypeEntity"/>
    public byte? SeasonId { get; set; }

    /// <summary>
    /// The category of the anime.
    /// </summary>
    public AnimeCategoryTypeEntity? Category { get; set; }

    /// <summary>
    /// The season the anime was or will be released in.
    /// </summary>
    /// <remarks>
    /// The season generally refers to the Japanese airing schedule.
    /// </remarks>
    public AnimeSeasonTypeEntity? Season { get; set; }
}
