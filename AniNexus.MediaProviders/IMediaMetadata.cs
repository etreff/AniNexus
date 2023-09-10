namespace AniNexus.MediaProviders;

/// <summary>
/// Defines metadata for a piece of media.
/// </summary>
public interface IMediaMetadata : IMediaMetadataLite
{
    /// <summary>
    /// The date at which the media started.
    /// </summary>
    FuzzyDate? StartDate { get; }

    /// <summary>
    /// The synopsis of the media.
    /// </summary>
    string? Synopsis { get; }

    /// <summary>
    /// The average user score of this series between 0 and 100.
    /// </summary>
    int? AverageScore { get; }

    /// <summary>
    /// The mean user score of this series between 0 and 100.
    /// </summary>
    int? MeanScore { get; }

    /// <summary>
    /// The duration of this piece of media.
    /// </summary>
    /// <remarks>
    /// This only applies to video/music.
    /// </remarks>
    TimeSpan? Duration { get; }

    /// <summary>
    /// The media tags for this media.
    /// </summary>
    IReadOnlyList<IMediaTag> Tags { get; }

    /// <summary>
    /// The studios that created this media.
    /// </summary>
    IReadOnlyList<IMediaStudio> Studios { get; }
}
