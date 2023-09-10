using System.Diagnostics.CodeAnalysis;
using AniNexus.MediaProviders.Anilist.Api;
using AniNexus.MediaProviders.Anilist.Client;
using AniNexus.MediaProviders.Anilist.Models;
using AniNexus.MediaProviders.MediaProvider;
using AniNexus.MediaProviders.MediaProvider.Contexts;
using Microsoft.Extensions.Logging;
using StrawberryShake;

namespace AniNexus.MediaProviders.Anilist;

internal sealed class AnilistMediaProvider : MediaProviderBase
{
    protected override string Name { get; } = "Anilist Media Provider";

    private readonly IAnilistClient _client;

    public AnilistMediaProvider(
        IAnilistClient client,
        ILogger<AnilistMediaProvider> logger)
        : base(logger)
    {
        _client = client;
    }

    [return: NotNullIfNotNull(nameof(metadata))]
    private AnilistMediaMetadataLite? MapMediaMetadataLite(IMetadataLite? metadata)
    {
        if (metadata is null)
        {
            return null;
        }

        return new AnilistMediaMetadataLite
        {
            CoverArt = new AnilistMediaCoverArt
            {
                Blob = null,
                LargeUrl = MakeNullableUri(metadata.CoverImage?.Large),
                MediumUrl = MakeNullableUri(metadata.CoverImage?.Medium)
            },
            EndDate = MakeNullableFuzzyDate(metadata.EndDate),
            Format = MediaFormatHelper.MapFormat(metadata.Format),
            Id = metadata.Id,
            Name = new AnilistMediaName
            {
                NativeName = metadata.Title!.Native!,
                EnglishName = metadata.Title.English,
                RomajiName = metadata.Title.Romaji
            },
            OfficialUrl = null,
            ProviderUrl = MakeNullableUri(metadata.SiteUrl)!,
            Season = (EMediaSeason?)metadata.Season,
            Status = MediaStatusHelper.MapMediaStatus(metadata.Status),
            TotalParts = metadata.Episodes,
            Year = metadata.SeasonYear
        };
    }

    [return: NotNullIfNotNull(nameof(metadata))]
    private AnilistMediaMetadata? MapMediaMetadata(IMetadata? metadata)
    {
        if (metadata is null)
        {
            return null;
        }

        return new AnilistMediaMetadata
        {
            AverageScore = metadata.AverageScore,
            CoverArt = new AnilistMediaCoverArt
            {
                Blob = null,
                LargeUrl = MakeNullableUri(metadata.CoverImage?.Large),
                MediumUrl = MakeNullableUri(metadata.CoverImage?.Medium)
            },
            Duration = metadata.Duration.HasValue ? TimeSpan.FromMinutes(metadata.Duration.Value) : null,
            EndDate = MakeNullableFuzzyDate(metadata.EndDate),
            Format = MediaFormatHelper.MapFormat(metadata.Format),
            Id = metadata.Id,
            MeanScore = metadata.MeanScore,
            Name = new AnilistMediaName
            {
                NativeName = metadata.Title!.Native!,
                EnglishName = metadata.Title.English,
                RomajiName = metadata.Title.Romaji
            },
            OfficialUrl = null,
            ProviderUrl = MakeNullableUri(metadata.SiteUrl)!,
            Season = (EMediaSeason?)metadata.Season,
            StartDate = MakeNullableFuzzyDate(metadata.StartDate),
            Status = MediaStatusHelper.MapMediaStatus(metadata.Status),
            Studios = metadata.Studios?.Nodes?.Select(static s => (IMediaStudio)new AnilistMediaStudio
            {
                IsAnimationStudio = s!.IsAnimationStudio,
                Name = s.Name,
                OfficialUrl = MakeNullableUri(s.SiteUrl)
            }).ToList() ?? new List<IMediaStudio>(0),
            Synopsis = metadata.Description,
            Tags = metadata.Tags?.Select(static t => (IMediaTag)new AnilistMediaTag
            {
                IsAdult = t!.IsAdult == true,
                Description = t.Description,
                Name = t.Name,
                Rank = t.Rank ?? 0
            }).ToList() ?? new List<IMediaTag>(0),
            TotalParts = metadata.Episodes,
            Year = metadata.SeasonYear
        };
    }

    private bool CheckOperationStatus(IOperationResult result, ContextBase context)
    {
        if (result.HasErrors())
        {
            context.LogError(Logger, result.BuildContextDictionary());
            return false;
        }

        return true;
    }

    protected override async Task<bool> AddMediaListEntry(AddMediaListEntryContext context, CancellationToken cancellationToken)
    {
        var response = await _client.CreateMediaListEntry.ExecuteAsync(context.ProviderMediaId, MediaStatusHelper.MapListStatus(context.Status), cancellationToken);
        return CheckOperationStatus(response, context);
    }

    protected override async Task<bool> DeleteMediaListEntry(DeleteMediaListEntryContext context, CancellationToken cancellationToken)
    {
        int mediaListEntryId;
        if (context.MediaListEntryId > 0)
        {
            mediaListEntryId = context.MediaListEntryId;
        }
        else
        {
            var mediaListEntry = await GetMediaListEntryAsync(context.ProviderMediaId, context.Format, context.Username, cancellationToken);
            if (mediaListEntry is null)
            {
                return true;
            }

            mediaListEntryId = mediaListEntry.Id;
        }

        var response = await _client.DeleteMediaListEntry.ExecuteAsync(mediaListEntryId, cancellationToken);
        return CheckOperationStatus(response, context);
    }

    public override async Task<IReadOnlyList<IAiringSeries>> GetAiringSeriesAsync(CancellationToken cancellationToken)
    {
        var alreadyReturned = new HashSet<int>();
        var result = new List<IAiringSeries>();

        await foreach (var entry in _client.EnumeratePagedResult(
            // Note: It is possible that we may get duplicate series if a series airs multiple episodes a week. We only care about the first one.
            static context => context.Client.GetAiringAnime.ExecuteAsync(new AiringSort[] { AiringSort.Episode }, AnilistDateHelper.AddTimeFromNow(TimeSpan.FromDays(14)), context.Page, context.PerPage, context.CancellationToken),
            static result => result.Page?.PageInfo,
            static result => result.Page?.AiringSchedules,
            Logger,
            cancellationToken))
        {
            var series = new AnilistAiringSeries(
                entry.MediaId,
                entry.Episode,
                entry.AiringAt,
                entry.TimeUntilAiring,
                MapMediaMetadataLite(entry.Media)!,
                entry.Media?.MediaListEntry is not null
                    ? new AnilistMediaListEntry(entry.Media.MediaListEntry)
                    : null);

            // Despite the confusing name, if this returns true it means it wasn't already
            // added yet.
            if (alreadyReturned.Add(series.MediaId))
            {
                result.Add(series);
            }
        }

        return result;
    }

    protected override async Task<IMediaCoverArt?> GetCoverArtAsync(GetCoverArtContext context, CancellationToken cancellationToken)
    {
        var response = await _client.GetCoverArt.ExecuteAsync(context.ProviderMediaId, MediaFormatHelper.GetMediaType(context.Format), cancellationToken);
        if (!CheckOperationStatus(response, context))
        {
            return null;
        }

        var coverArt = response.Data?.Media?.CoverImage;
        return coverArt is not null
            ? new AnilistMediaCoverArt
            {
                LargeUrl = MakeNullableUri(coverArt.Large),
                MediumUrl = MakeNullableUri(coverArt.Medium)
            } : null;
    }

    protected override async Task<IMediaListEntry?> GetMediaListEntryAsync(GetMediaListEntryContext context, CancellationToken cancellationToken)
    {
        var response = await _client.GetMediaListEntry.ExecuteAsync(context.ProviderMediaId, context.Username, MediaFormatHelper.GetMediaType(context.Format), cancellationToken);
        if (!CheckOperationStatus(response, context))
        {
            return null;
        }

        var entry = response.Data?.MediaList;
        return entry is not null
            ? new AnilistMediaListEntry(entry)
            : null;
    }

    protected override async Task<IList<IMediaListEntry>> GetMediaListEntriesAsync(GetMediaListEntriesContext context, CancellationToken cancellationToken)
    {
        var result = new List<IMediaListEntry>();

        if (context.ProviderMediaIds is not null)
        {
            int[] ids = context.ProviderMediaIds.ToArray();

            await foreach (var entry in _client.EnumeratePagedResult(
                c => c.Client.GetMediaListEntriesById.ExecuteAsync(ids, context.Username, MediaFormatHelper.GetMediaType(context.Format), c.Page, c.PerPage, c.CancellationToken),
                static c => c.Page?.PageInfo,
                static c => c.Page?.MediaList,
                Logger,
                cancellationToken
            ))
            {
                result.Add(new AnilistMediaListEntry(entry));
            }
        }
        else
        {
            var response = await _client.GetMediaListEntries.ExecuteAsync(context.Username, MediaFormatHelper.GetMediaType(context.Format), cancellationToken);
            if (!CheckOperationStatus(response, context))
            {
                return result;
            }

            var lists = response.Data?.MediaListCollection?.Lists;
            if (lists is null)
            {
                return result;
            }

            foreach (var list in lists)
            {
                if (list?.Entries is null)
                {
                    continue;
                }

                foreach (var entry in list.Entries)
                {
                    if (entry is null)
                    {
                        continue;
                    }

                    result.Add(new AnilistMediaListEntry(entry));
                }
            }
        }

        return result;
    }

    protected override async Task<IMediaMetadata?> GetMetadataAsync(GetMetadataContext context, CancellationToken cancellationToken)
    {
        var response = await _client.GetMetadataById.ExecuteAsync(context.ProviderMediaId, MediaFormatHelper.GetMediaType(context.Format), cancellationToken);
        if (!CheckOperationStatus(response, context))
        {
            return null;
        }

        var metadata = response.Data?.Media;
        return metadata is not null
            ? MapMediaMetadata(metadata)
            : null;
    }

    protected override async Task<IMediaMetadataLite?> GetMetadataLiteAsync(GetMetadataLiteContext context, CancellationToken cancellationToken)
    {
        var response = await _client.GetMetadataLiteById.ExecuteAsync(context.ProviderMediaId, MediaFormatHelper.GetMediaType(context.Format), cancellationToken);
        if (!CheckOperationStatus(response, context))
        {
            return null;
        }

        var metadata = response.Data?.Media;
        return metadata is not null
            ? MapMediaMetadataLite(metadata)
            : null;
    }

    protected override async Task<IReadOnlyList<IMediaMetadata>> SearchMetadataAsync(SearchMetadataContext context, CancellationToken cancellationToken)
    {
        var results = new List<IMediaMetadata>();
        var format = MediaFormatHelper.GetMediaType(context.Format);

        if (!format.HasValue || format == MediaType.Anime)
        {
            await foreach (var metadata in _client.EnumeratePagedResult(
                c => c.Client.SearchMetadataByName.ExecuteAsync(context.MediaName, MediaType.Anime, c.Page, c.PerPage, c.CancellationToken),
                static c => c.Page?.PageInfo,
                static c => c.Page?.Media,
                Logger,
                cancellationToken
            ))
            {
                if (metadata is null)
                {
                    continue;
                }

                results.Add(MapMediaMetadata(metadata));
            }
        }

        if (!format.HasValue || format == MediaType.Manga)
        {
            await foreach (var metadata in _client.EnumeratePagedResult(
                c => c.Client.SearchMetadataByName.ExecuteAsync(context.MediaName, MediaType.Manga, c.Page, c.PerPage, c.CancellationToken),
                static c => c.Page?.PageInfo,
                static c => c.Page?.Media,
                Logger,
                cancellationToken
            ))
            {
                if (metadata is null)
                {
                    continue;
                }

                results.Add(MapMediaMetadata(metadata));
            }
        }

        return results;
    }

    protected override async Task<IReadOnlyList<IMediaMetadataLite>> SearchMetadataLiteAsync(SearchMetadataContext context, CancellationToken cancellationToken)
    {
        var results = new List<IMediaMetadataLite>();
        var format = MediaFormatHelper.GetMediaType(context.Format);

        if (!format.HasValue || format == MediaType.Anime)
        {
            await foreach (var metadata in _client.EnumeratePagedResult(
                c => c.Client.SearchMetadataLiteByName.ExecuteAsync(context.MediaName, MediaType.Anime, c.Page, c.PerPage, c.CancellationToken),
                static c => c.Page?.PageInfo,
                static c => c.Page?.Media,
                Logger,
                cancellationToken
            ))
            {
                if (metadata is null)
                {
                    continue;
                }

                results.Add(MapMediaMetadataLite(metadata));
            }
        }

        if (!format.HasValue || format == MediaType.Manga)
        {
            await foreach (var metadata in _client.EnumeratePagedResult(
                c => c.Client.SearchMetadataLiteByName.ExecuteAsync(context.MediaName, MediaType.Manga, c.Page, c.PerPage, c.CancellationToken),
                static c => c.Page?.PageInfo,
                static c => c.Page?.Media,
                Logger,
                cancellationToken
            ))
            {
                if (metadata is null)
                {
                    continue;
                }

                results.Add(MapMediaMetadataLite(metadata));
            }
        }

        return results;
    }

    protected override async Task<bool> UpdateMediaListEntryProgressAndStatusAsync(UpdateMediaListEntryContext context, CancellationToken cancellationToken)
    {
        int mediaListEntryId = context.MediaListEntryId;
        if (mediaListEntryId == 0)
        {
            var mediaListEntry = await GetMediaListEntryAsync(context.ProviderMediaId, context.Format, context.Username, cancellationToken);
            if (mediaListEntry is null)
            {
                return false;
            }

            mediaListEntryId = mediaListEntry.Id;
        }

        var response = await _client.UpdateMediaListEntryProgressAndStatus.ExecuteAsync(mediaListEntryId, context.Progress, MediaStatusHelper.MapListStatus(context.Status), cancellationToken);
        return CheckOperationStatus(response, context);
    }

    protected override async Task<bool> UpdateMediaListEntryProgressAsync(UpdateMediaListEntryContext context, CancellationToken cancellationToken)
    {
        int mediaListEntryId = context.MediaListEntryId;
        if (mediaListEntryId == 0)
        {
            var mediaListEntry = await GetMediaListEntryAsync(context.ProviderMediaId, context.Format, context.Username, cancellationToken);
            if (mediaListEntry is null)
            {
                return false;
            }

            mediaListEntryId = mediaListEntry.Id;
        }

        var response = await _client.UpdateMediaListEntryProgress.ExecuteAsync(mediaListEntryId, context.Progress, cancellationToken);
        return CheckOperationStatus(response, context);
    }

    protected override async Task<bool> UpdateMediaListEntryStatusAsync(UpdateMediaListEntryContext context, CancellationToken cancellationToken)
    {
        int mediaListEntryId = context.MediaListEntryId;
        if (mediaListEntryId == 0)
        {
            var mediaListEntry = await GetMediaListEntryAsync(context.ProviderMediaId, context.Format, context.Username, cancellationToken);
            if (mediaListEntry is null)
            {
                return false;
            }

            mediaListEntryId = mediaListEntry.Id;
        }

        var response = await _client.UpdateMediaListEntryStatus.ExecuteAsync(mediaListEntryId, MediaStatusHelper.MapListStatus(context.Status), cancellationToken);
        return CheckOperationStatus(response, context);
    }

    protected override async Task<bool> UpdateMediaListEntryScoreAsync(UpdateMediaListEntryScoreContext context, CancellationToken cancellationToken)
    {
        int mediaListEntryId = context.MediaListEntryId;
        if (mediaListEntryId == 0)
        {
            var mediaListEntry = await GetMediaListEntryAsync(context.ProviderMediaId, context.Format, context.Username, cancellationToken);
            if (mediaListEntry is null)
            {
                return false;
            }

            mediaListEntryId = mediaListEntry.Id;
        }

        // Divide score by 10 since score is 0-100.
        var response = await _client.UpdateMediaListEntryScores.ExecuteAsync(mediaListEntryId, context.Score.GetScoreList(), context.Score.Overall / 10.0, cancellationToken);
        return CheckOperationStatus(response, context);
    }
}
