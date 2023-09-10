namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Models a manga series.
/// </summary>
public sealed class SeriesMangaEntity : SeriesMediaEntity
{
    /// <summary>
    /// The category of the manga.
    /// </summary>
    public MangaCategoryTypeEntity? Category { get; set; }
}
