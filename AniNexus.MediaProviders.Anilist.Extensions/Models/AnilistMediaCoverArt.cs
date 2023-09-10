namespace AniNexus.MediaProviders.Anilist.Models;

internal sealed class AnilistMediaCoverArt : IMediaCoverArt
{
    public required Uri? MediumUrl { get; init; }

    public required Uri? LargeUrl { get; init; }

    public byte[]? Blob { get; init; }
}
