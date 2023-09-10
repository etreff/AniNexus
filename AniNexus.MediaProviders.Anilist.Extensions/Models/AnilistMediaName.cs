namespace AniNexus.MediaProviders.Anilist.Models;

internal sealed class AnilistMediaName : IMediaName
{
    public required string NativeName { get; init; }

    public string? EnglishName { get; init; }

    public string? RomajiName { get; init; }
}
