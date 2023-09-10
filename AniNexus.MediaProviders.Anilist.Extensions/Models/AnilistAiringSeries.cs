namespace AniNexus.MediaProviders.Anilist;

internal sealed class AnilistAiringSeries : IAiringSeries
{
    public int MediaId { get; }

    public decimal Episode { get; }

    public DateTime AiringAt { get; }

    public TimeSpan TimeUntilAirs { get; }

    public IMediaMetadataLite Metadata { get; }

    public IMediaListEntry? MediaListEntry { get; }

    public AnilistAiringSeries(int mediaId, decimal episode, int airingAt, int timeUntilAirs, IMediaMetadataLite metadata, IMediaListEntry? mediaListEntry)
    {
        MediaId = mediaId;
        Episode = episode;
        AiringAt = AnilistDateHelper.GetAiringAtDateTime(airingAt);
        TimeUntilAirs = AnilistDateHelper.GetTimeUntilAiring(timeUntilAirs);
        Metadata = metadata;
        MediaListEntry = mediaListEntry;
    }
}
