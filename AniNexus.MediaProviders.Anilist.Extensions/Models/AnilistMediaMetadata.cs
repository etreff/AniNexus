namespace AniNexus.MediaProviders.Anilist.Models;

internal sealed class AnilistMediaMetadata : AnilistMediaMetadataLite, IMediaMetadata
{
    public required FuzzyDate? StartDate { get; init; }

    public required string? Synopsis { get; init; }

    public required int? AverageScore { get; init; }

    public required int? MeanScore { get; init; }

    public required TimeSpan? Duration { get; init; }

    public required IReadOnlyList<IMediaTag> Tags { get; init; }

    public required IReadOnlyList<IMediaStudio> Studios { get; init; }
}
