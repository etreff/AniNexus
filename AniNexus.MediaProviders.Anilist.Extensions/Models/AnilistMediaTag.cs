namespace AniNexus.MediaProviders.Anilist.Models;

internal sealed class AnilistMediaTag : IMediaTag
{
    public required string Name { get; init; }

    public string? Description { get; init; }

    public int Rank { get; init; }

    public bool IsAdult { get; init; }
}
