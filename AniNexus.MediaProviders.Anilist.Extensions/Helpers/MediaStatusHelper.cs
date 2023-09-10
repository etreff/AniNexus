using AniNexus.MediaProviders.Anilist.Client;

namespace AniNexus.MediaProviders.Anilist;

internal static class MediaStatusHelper
{
    public static EMediaListStatus MapListStatus(MediaListStatus? status) => status switch
    {
        MediaListStatus.Completed => EMediaListStatus.Completed,
        MediaListStatus.Current => EMediaListStatus.Current,
        MediaListStatus.Dropped => EMediaListStatus.Dropped,
        MediaListStatus.Paused => EMediaListStatus.Paused,
        MediaListStatus.Planning => EMediaListStatus.Planning,
        MediaListStatus.Repeating => EMediaListStatus.Repeating,
        _ => EMediaListStatus.Planning
    };

    public static MediaListStatus MapListStatus(EMediaListStatus? status) => status switch
    {
        EMediaListStatus.Completed => MediaListStatus.Completed,
        EMediaListStatus.Current => MediaListStatus.Current,
        EMediaListStatus.Dropped => MediaListStatus.Dropped,
        EMediaListStatus.Paused => MediaListStatus.Paused,
        EMediaListStatus.Planning => MediaListStatus.Planning,
        EMediaListStatus.Repeating => MediaListStatus.Repeating,
        _ => MediaListStatus.Planning
    };

    public static EMediaStatus MapMediaStatus(MediaStatus? status) => status switch
    {
        MediaStatus.Cancelled => EMediaStatus.Canceled,
        MediaStatus.Finished => EMediaStatus.Finished,
        MediaStatus.Hiatus => EMediaStatus.Hiatus,
        MediaStatus.NotYetReleased => EMediaStatus.NotYetReleased,
        MediaStatus.Releasing => EMediaStatus.Releasing,
        _ => EMediaStatus.NotYetReleased
    };
}
