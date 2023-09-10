namespace AniNexus.MediaProviders.Anilist;

internal sealed class AnilistMediaListEntry : IMediaListEntry
{
    public int Id { get; }

    public int MediaId { get; }

    public int? MalId { get; }
    public string MediaName { get; }

    public decimal? Score { get; }

    public int Repeat { get; }

    public int Progress { get; }

    public string? Notes { get; }

    public EMediaListStatus Status { get; }

    public EMediaFormat MediaFormat { get; }

    public DateOnly? StartedAt { get; }

    public DateOnly? CompletedAt { get; }

    public AnilistMediaListEntry(Anilist.Client.IMediaListEntry entry)
    {
        Id = entry.Id;
        MediaId = entry.MediaId;
        MalId = entry.Media!.IdMal;
        MediaName = entry.Media.Title!.Romaji ?? entry.Media.Title!.English ?? string.Empty;
        MediaFormat = MediaFormatHelper.MapFormat(entry.Media.Format);
        Score = (decimal?)entry.Score;
        Repeat = entry.Repeat ?? 0;
        Progress = entry.Progress ?? 0;
        Notes = entry.Notes;
        Status = MediaStatusHelper.MapListStatus(entry.Status);
        StartedAt = ToDateOnly(entry.StartedAt?.Year, entry.StartedAt?.Month, entry.StartedAt?.Day);
        CompletedAt = ToDateOnly(entry.CompletedAt?.Year, entry.CompletedAt?.Month, entry.CompletedAt?.Day);
    }

    private static DateOnly? ToDateOnly(int? year, int? month, int? day)
    {
        return year.HasValue && month.HasValue && day.HasValue
            ? new DateOnly(year.Value, month.Value, day.Value)
            : null;
    }
}
