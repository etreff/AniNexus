using System.Diagnostics.CodeAnalysis;
using AniNexus.MediaProviders.Anilist.Client;

namespace AniNexus.MediaProviders.Anilist;

internal static class MediaFormatHelper
{
    public static EMediaFormat MapFormat(MediaFormat? format) => format switch
    {
        MediaFormat.Manga => EMediaFormat.Manga,
        MediaFormat.Movie => EMediaFormat.Movie,
        MediaFormat.Music => EMediaFormat.Music,
        MediaFormat.Novel => EMediaFormat.Novel,
        MediaFormat.Ona => EMediaFormat.Ona,
        MediaFormat.OneShot => EMediaFormat.OneShot,
        MediaFormat.Ova => EMediaFormat.Ova,
        MediaFormat.Special => EMediaFormat.Special,
        MediaFormat.Tv => EMediaFormat.Tv,
        MediaFormat.TvShort => EMediaFormat.TvShort,
        _ => EMediaFormat.Tv
    };

    public static MediaType GetMediaType(EMediaFormat format) => format switch
    {
        EMediaFormat.Manga or
        EMediaFormat.Novel or
        EMediaFormat.OneShot => MediaType.Manga,
        _ => MediaType.Anime
    };

    [return: NotNullIfNotNull(nameof(format))]
    public static MediaType? GetMediaType(EMediaFormat? format) => format switch
    {
        null => null,
        _ => GetMediaType(format.Value)
    };
}
