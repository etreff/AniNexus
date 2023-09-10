using AniNexus.DataAccess.Entities.Owned;

namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Models a media series.
/// </summary>
public abstract class SeriesMediaEntity : Entity<SeriesMediaEntity, int>
{
    /// <summary>
    /// The name of this media entry.
    /// </summary>
    public required MediaTitleEntity Name { get; set; }

    /// <summary>
    /// The category/type of this series.
    /// </summary>
    public required byte CategoryId { get; set; }

    /// <summary>
    /// The official website of this media entry.
    /// </summary>
    public string? WebsiteUrl { get; set; }

    /// <summary>
    /// A synopsis or description of the media entity.
    /// </summary>
    public string? Synopsis { get; set; }

    /// <summary>
    /// The average user rating for this media, between 0 and 100.
    /// The rating only takes into account ratings from users who have
    /// given the media a rating and have completed the media.
    /// </summary>
    /// <remarks>
    /// This value will be calculated by the system periodically.
    /// </remarks>
    public byte? Rating { get; set; }

    /// <summary>
    /// The average user rating for this media, between 0 and 100.
    /// The rating only takes into account ratings from users who have given
    /// the media a rating and are actively watching the media.
    /// </summary>
    /// <remarks>
    /// This value will be calculated by the system periodically.
    /// </remarks>
    public byte? ActiveRating { get; set; }

    /// <summary>
    /// The number of user votes that contributed to <see cref="Rating"/>.
    /// </summary>
    /// <remarks>
    /// This value will be calculated by the system periodically.
    /// </remarks>
    public int Votes { get; set; }
}
