namespace AniNexus.MediaProviders.MediaProvider;

/// <summary>
/// Defines a service that provides media information.
/// </summary>
public interface IMediaProvider
{
    /// <summary>
    /// Gets the metadata for the media with the specified Id and format.
    /// </summary>
    /// <param name="providerMediaId">The Id of the media in the provider's system.</param>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The metadata if found, <see langword="null"/> otherwise.</returns>
    Task<IMediaMetadata?> GetMetadataAsync(int providerMediaId, EMediaFormat format, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the minimal metadata for the media with the specified Id and format.
    /// </summary>
    /// <param name="providerMediaId">The Id of the media in the provider's system.</param>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The metadata if found, <see langword="null"/> otherwise.</returns>
    Task<IMediaMetadataLite?> GetMetadataLiteAsync(int providerMediaId, EMediaFormat format, CancellationToken cancellationToken);

    /// <summary>
    /// Searches the provider for media metadata that matches the provided series name and format.
    /// </summary>
    /// <param name="seriesName">The name of the series to search for.</param>
    /// <param name="format">The format of the media to look for. If <see langword="null"/>, all media formats will be searched.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<IReadOnlyList<IMediaMetadata>> SearchMetadataAsync(string seriesName, EMediaFormat? format, CancellationToken cancellationToken);

    /// <summary>
    /// Searches the provider for media metadata that matches the provided series name and format.
    /// </summary>
    /// <param name="seriesName">The name of the series to search for.</param>
    /// <param name="format">The format of the media to look for. If <see langword="null"/>, all media formats will be searched.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<IReadOnlyList<IMediaMetadataLite>> SearchMetadataLiteAsync(string seriesName, EMediaFormat? format, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the cover art for the media with the specified Id.
    /// </summary>
    /// <param name="providerMediaId">The Id of the media in the provider's system.</param>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The cover art if found, <see langword="null"/> otherwise.</returns>
    Task<IMediaCoverArt?> GetCoverArtAsync(int providerMediaId, EMediaFormat format, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the airing series from the provider.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<IReadOnlyList<IAiringSeries>> GetAiringSeriesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets the user's media list entry for the series with the specified Id and format.
    /// </summary>
    /// <param name="providerMediaId">The Id of the media in the provider's system.</param>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="username">The username of the user to get the list entry for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The media list entry if found, <see langword="null"/> otherwise.</returns>
    Task<IMediaListEntry?> GetMediaListEntryAsync(int providerMediaId, EMediaFormat format, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all of the user's media list entries for the media with the specified format.
    /// </summary>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="username">The username of the user to get the list entries for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<IList<IMediaListEntry>> GetMediaListEntriesAsync(EMediaFormat format, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the user's media list entries with the specified Ids for the media with the specified format.
    /// </summary>
    /// <param name="providerMediaIds">The Ids of the entries to get the data for.</param>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="username">The username of the user to get the list entries for.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<IList<IMediaListEntry>> GetMediaListEntriesAsync(IEnumerable<int> providerMediaIds, EMediaFormat format, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Adds a new media list entry to the specified user's media list.
    /// </summary>
    /// <param name="providerMediaId">The Id of the media in the provider's system.</param>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="status">The status to set the media list entry to.</param>
    /// <param name="username">The username of the user to add the list entry to.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> AddMediaListEntry(int providerMediaId, EMediaFormat format, EMediaListStatus status, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the media list entry from the user's media list with the specified media list entry Id.
    /// </summary>
    /// <param name="mediaListEntryId">The Id of the media list entry in the provider's system.</param>
    /// <param name="username">The username of the user to delete the list entry from.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> DeleteMediaListEntry(int mediaListEntryId, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the media list entry from the user's media list with the specified media Id.
    /// </summary>
    /// <param name="providerMediaId">The Id of the media in the provider's system.</param>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="username">The username of the user to delete the list entry from.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> DeleteMediaListEntry(int providerMediaId, EMediaFormat format, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the progress of the media list entry with the specified Id.
    /// </summary>
    /// <param name="mediaListEntryId">The Id of the media list entry in the provider's system.</param>
    /// <param name="newProgress">The new progress value.</param>
    /// <param name="username">The username of the user to update the list entry of.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> UpdateMediaListEntryProgressAsync(int mediaListEntryId, int newProgress, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the progress of the media list entry with the specified Id.
    /// </summary>
    /// <param name="providerMediaId">The Id of the media in the provider's system.</param>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="newProgress">The new progress value.</param>
    /// <param name="username">The username of the user to update the list entry of.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> UpdateMediaListEntryProgressAsync(int providerMediaId, EMediaFormat format, int newProgress, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the status of the media list entry with the specified Id.
    /// </summary>
    /// <param name="mediaListEntryId">The Id of the media list entry in the provider's system.</param>
    /// <param name="newStatus">The new status value.</param>
    /// <param name="username">The username of the user to update the list entry of.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> UpdateMediaListEntryStatusAsync(int mediaListEntryId, EMediaListStatus newStatus, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the status of the media list entry with the specified Id.
    /// </summary>
    /// <param name="providerMediaId">The Id of the media in the provider's system.</param>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="newStatus">The new status value.</param>
    /// <param name="username">The username of the user to update the list entry of.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> UpdateMediaListEntryStatusAsync(int providerMediaId, EMediaFormat format, EMediaListStatus newStatus, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the progress and status of the media list entry with the specified Id.
    /// </summary>
    /// <param name="mediaListEntryId">The Id of the media list entry in the provider's system.</param>
    /// <param name="newProgress">The new progress value.</param>
    /// <param name="newStatus">The new status value.</param>
    /// <param name="username">The username of the user to update the list entry of.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> UpdateMediaListEntryProgressAndStatusAsync(int mediaListEntryId, int newProgress, EMediaListStatus newStatus, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the progress and status of the media list entry with the specified Id.
    /// </summary>
    /// <param name="providerMediaId">The Id of the media in the provider's system.</param>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="newProgress">The new progress value.</param>
    /// <param name="newStatus">The new status value.</param>
    /// <param name="username">The username of the user to update the list entry of.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> UpdateMediaListEntryProgressAndStatusAsync(int providerMediaId, EMediaFormat format, int newProgress, EMediaListStatus newStatus, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the scores for the media list entry with the specified Id.
    /// </summary>
    /// <param name="mediaListEntryId">The Id of the media list entry in the provider's system.</param>
    /// <param name="score">The score values.</param>
    /// <param name="username">The username of the user to update the list entry of.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> UpdateMediaListEntryScoreAsync(int mediaListEntryId, MediaScore score, string username, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the scores for the media list entry with the specified Id.
    /// </summary>
    /// <param name="providerMediaId">The Id of the media in the provider's system.</param>
    /// <param name="format">The format of the media to look for.</param>
    /// <param name="score">The score values.</param>
    /// <param name="username">The username of the user to update the list entry of.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<bool> UpdateMediaListEntryScoreAsync(int providerMediaId, EMediaFormat format, MediaScore score, string username, CancellationToken cancellationToken);
}
