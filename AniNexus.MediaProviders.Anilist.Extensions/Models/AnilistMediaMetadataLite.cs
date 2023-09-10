namespace AniNexus.MediaProviders.Anilist.Models;

internal class AnilistMediaMetadataLite : IMediaMetadataLite
{
    public required int Id { get; init; }

    public required IMediaName Name { get; init; }

    public required EMediaSeason? Season { get; init; }

    public required int? TotalParts { get; init; }

    public required EMediaFormat Format { get; init; }

    public required EMediaStatus Status { get; init; }

    public required Uri ProviderUrl { get; init; }

    public Uri? OfficialUrl { get; init; }

    public required IMediaCoverArt CoverArt { get; init; }

    public required FuzzyDate? EndDate { get; init; }

    public required int? Year { get; init; }

    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
