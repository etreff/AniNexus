namespace AniNexus.MediaProviders.Anilist.Models;

internal sealed class AnilistMediaStudio : IMediaStudio
{
    public required string Name { get; init; }

    public bool IsAnimationStudio { get; init; }

    public Uri? ProviderUrl { get; init; }

    public Uri? OfficialUrl { get; init; }
}
