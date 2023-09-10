using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using AniNexus.MediaProviders.MediaProvider.Contexts;
using Microsoft.Extensions.Logging;

namespace AniNexus.MediaProviders.MediaProvider;

/// <summary>
/// The base class for a <see cref="IMediaProvider"/>.
/// </summary>
public abstract class MediaProviderBase : IMediaProvider
{
    /// <summary>
    /// The name of this provider.
    /// </summary>
    protected abstract string Name { get; }

    /// <summary>
    /// The logger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Creates a new <see cref="MediaProviderBase"/> instance.
    /// </summary>
    /// <param name="logger">The logger.</param>
    protected MediaProviderBase(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Creates a <see cref="Uri"/> out of <paramref name="uri"/>
    /// if the value is a valid value.
    /// </summary>
    /// <param name="uri">The URI to create.</param>
    /// <returns>A <see cref="Uri"/> instance if <paramref name="uri"/> is valid, <see langword="null"/> otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(uri))]
    protected static Uri? MakeNullableUri(string? uri)
    {
        return Uri.TryCreate(uri, UriKind.Absolute, out var result)
            ? result
            : null;
    }

    /// <summary>
    /// Creates a <see cref="FuzzyDate"/> out of a year, month, and day
    /// if the values would create a valid <see cref="FuzzyDate"/> instance.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <param name="month">The month.</param>
    /// <param name="day">The day.</param>
    /// <returns>A <see cref="FuzzyDate"/> instance if the year, month, and day make a valid <see cref="FuzzyDate"/>, <see langword="null"/> otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(year))]
    [return: NotNullIfNotNull(nameof(month))]
    protected static FuzzyDate? MakeNullableFuzzyDate(int? year, int? month, int? day)
    {
        return FuzzyDate.TryCreate(year, month, day, out var result)
            ? result
            : null;
    }

    /// <summary>
    /// Creates a <see cref="FuzzyDate"/> out of the Year, Month, and Day
    /// properties of an object if the values would create a valid <see cref="FuzzyDate"/> instance.
    /// </summary>
    /// <param name="date">The object containing the date properties.</param>
    /// <returns>The <see cref="FuzzyDate"/> if one could be generated, <see langword="null"/> otherwise.</returns>
    protected FuzzyDate? MakeNullableFuzzyDate(dynamic? date)
    {
        if (date == null)
        {
            return null;
        }

        try
        {
            return MakeNullableFuzzyDate(date.Year, date.Month, date.Day);
        }
        catch
        {
            return null;
        }
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public Task<bool> AddMediaListEntry(int providerMediaId, EMediaFormat format, EMediaListStatus status, string username, CancellationToken cancellationToken)
    {
        var context = new AddMediaListEntryContext(providerMediaId, format, status, username);
        return AddMediaListEntry(context, cancellationToken);
    }
    protected abstract Task<bool> AddMediaListEntry(AddMediaListEntryContext context, CancellationToken cancellationToken);

    public Task<bool> DeleteMediaListEntry(int mediaListEntryId, string username, CancellationToken cancellationToken)
    {
        var context = new DeleteMediaListEntryContext(mediaListEntryId, username);
        return DeleteMediaListEntry(context, cancellationToken);
    }
    public Task<bool> DeleteMediaListEntry(int providerMediaId, EMediaFormat format, string username, CancellationToken cancellationToken)
    {
        var context = new DeleteMediaListEntryContext(providerMediaId, format, username);
        return DeleteMediaListEntry(context, cancellationToken);
    }
    protected abstract Task<bool> DeleteMediaListEntry(DeleteMediaListEntryContext context, CancellationToken cancellationToken);

    public abstract Task<IReadOnlyList<IAiringSeries>> GetAiringSeriesAsync(CancellationToken cancellationToken);

    public Task<IMediaCoverArt?> GetCoverArtAsync(int providerMediaId, EMediaFormat format, CancellationToken cancellationToken)
    {
        var context = new GetCoverArtContext(providerMediaId, format);
        return GetCoverArtAsync(context, cancellationToken);
    }
    protected abstract Task<IMediaCoverArt?> GetCoverArtAsync(GetCoverArtContext context, CancellationToken cancellationToken);

    public Task<IMediaListEntry?> GetMediaListEntryAsync(int providerMediaId, EMediaFormat format, string username, CancellationToken cancellationToken)
    {
        var context = new GetMediaListEntryContext(providerMediaId, format, username);
        return GetMediaListEntryAsync(context, cancellationToken);
    }
    protected abstract Task<IMediaListEntry?> GetMediaListEntryAsync(GetMediaListEntryContext context, CancellationToken cancellationToken);

    public Task<IList<IMediaListEntry>> GetMediaListEntriesAsync(EMediaFormat format, string username, CancellationToken cancellationToken)
    {
        var context = new GetMediaListEntriesContext(null, format, username);
        return GetMediaListEntriesAsync(context, cancellationToken);
    }
    public Task<IList<IMediaListEntry>> GetMediaListEntriesAsync(IEnumerable<int> providerMediaIds, EMediaFormat format, string username, CancellationToken cancellationToken)
    {
        var context = new GetMediaListEntriesContext(providerMediaIds, format, username);
        return GetMediaListEntriesAsync(context, cancellationToken);
    }
    protected abstract Task<IList<IMediaListEntry>> GetMediaListEntriesAsync(GetMediaListEntriesContext context, CancellationToken cancellationToken);

    public Task<IMediaMetadata?> GetMetadataAsync(int providerMediaId, EMediaFormat format, CancellationToken cancellationToken)
    {
        var context = new GetMetadataContext(providerMediaId, format);
        return GetMetadataAsync(context, cancellationToken);
    }
    protected abstract Task<IMediaMetadata?> GetMetadataAsync(GetMetadataContext context, CancellationToken cancellationToken);

    public Task<IMediaMetadataLite?> GetMetadataLiteAsync(int providerMediaId, EMediaFormat format, CancellationToken cancellationToken)
    {
        var context = new GetMetadataLiteContext(providerMediaId, format);
        return GetMetadataLiteAsync(context, cancellationToken);
    }
    protected abstract Task<IMediaMetadataLite?> GetMetadataLiteAsync(GetMetadataLiteContext context, CancellationToken cancellationToken);

    public Task<IReadOnlyList<IMediaMetadata>> SearchMetadataAsync(string seriesName, EMediaFormat? format, CancellationToken cancellationToken)
    {
        var context = new SearchMetadataContext(seriesName, format);
        return SearchMetadataAsync(context, cancellationToken);
    }
    protected abstract Task<IReadOnlyList<IMediaMetadata>> SearchMetadataAsync(SearchMetadataContext context, CancellationToken cancellationToken);

    public Task<IReadOnlyList<IMediaMetadataLite>> SearchMetadataLiteAsync(string seriesName, EMediaFormat? format, CancellationToken cancellationToken)
    {
        var context = new SearchMetadataContext(seriesName, format);
        return SearchMetadataLiteAsync(context, cancellationToken);
    }
    protected abstract Task<IReadOnlyList<IMediaMetadataLite>> SearchMetadataLiteAsync(SearchMetadataContext context, CancellationToken cancellationToken);

    public Task<bool> UpdateMediaListEntryProgressAndStatusAsync(int mediaListEntryId, int newProgress, EMediaListStatus newStatus, string username, CancellationToken cancellationToken)
    {
        var context = new UpdateMediaListEntryContext(mediaListEntryId, newStatus, newProgress, username);
        return UpdateMediaListEntryProgressAndStatusAsync(context, cancellationToken);
    }
    public Task<bool> UpdateMediaListEntryProgressAndStatusAsync(int providerMediaId, EMediaFormat format, int newProgress, EMediaListStatus newStatus, string username, CancellationToken cancellationToken)
    {
        var context = new UpdateMediaListEntryContext(providerMediaId, format, newStatus, newProgress, username);
        return UpdateMediaListEntryProgressAndStatusAsync(context, cancellationToken);
    }
    protected abstract Task<bool> UpdateMediaListEntryProgressAndStatusAsync(UpdateMediaListEntryContext context, CancellationToken cancellationToken);

    public Task<bool> UpdateMediaListEntryProgressAsync(int mediaListEntryId, int newProgress, string username, CancellationToken cancellationToken)
    {
        var context = new UpdateMediaListEntryContext(mediaListEntryId, default, newProgress, username);
        return UpdateMediaListEntryProgressAsync(context, cancellationToken);
    }
    public Task<bool> UpdateMediaListEntryProgressAsync(int providerMediaId, EMediaFormat format, int newProgress, string username, CancellationToken cancellationToken)
    {
        var context = new UpdateMediaListEntryContext(providerMediaId, format, default, newProgress, username);
        return UpdateMediaListEntryProgressAsync(context, cancellationToken);
    }
    protected abstract Task<bool> UpdateMediaListEntryProgressAsync(UpdateMediaListEntryContext context, CancellationToken cancellationToken);

    public Task<bool> UpdateMediaListEntryScoreAsync(int mediaListEntryId, MediaScore score, string username, CancellationToken cancellationToken)
    {
        var context = new UpdateMediaListEntryScoreContext(mediaListEntryId, score, username);
        return UpdateMediaListEntryScoreAsync(context, cancellationToken);
    }
    public Task<bool> UpdateMediaListEntryScoreAsync(int providerMediaId, EMediaFormat format, MediaScore score, string username, CancellationToken cancellationToken)
    {
        var context = new UpdateMediaListEntryScoreContext(providerMediaId, format, score, username);
        return UpdateMediaListEntryScoreAsync(context, cancellationToken);
    }
    protected abstract Task<bool> UpdateMediaListEntryScoreAsync(UpdateMediaListEntryScoreContext context, CancellationToken cancellationToken);

    public Task<bool> UpdateMediaListEntryStatusAsync(int mediaListEntryId, EMediaListStatus newStatus, string username, CancellationToken cancellationToken)
    {
        var context = new UpdateMediaListEntryContext(mediaListEntryId, newStatus, default, username);
        return UpdateMediaListEntryStatusAsync(context, cancellationToken);
    }
    public Task<bool> UpdateMediaListEntryStatusAsync(int providerMediaId, EMediaFormat format, EMediaListStatus newStatus, string username, CancellationToken cancellationToken)
    {
        var context = new UpdateMediaListEntryContext(providerMediaId, format, newStatus, default, username);
        return UpdateMediaListEntryStatusAsync(context, cancellationToken);
    }
    protected abstract Task<bool> UpdateMediaListEntryStatusAsync(UpdateMediaListEntryContext context, CancellationToken cancellationToken);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
