using Microsoft.Extensions.Logging;

namespace AniNexus.MediaProviders.MediaProvider.Contexts;

/// <summary>
/// The context for updating a media list entry in a media provider.
/// </summary>
public sealed class UpdateMediaListEntryContext : ContextBase
{
    /// <summary>
    /// The Id of the media list entry.
    /// </summary>
    public int MediaListEntryId { get; }

    /// <summary>
    /// The Id that the provider has assigned the media.
    /// </summary>
    public int ProviderMediaId { get; }

    /// <summary>
    /// The format of the requested <see cref="ProviderMediaId"/>.
    /// </summary>
    public EMediaFormat Format { get; }

    /// <summary>
    /// The status to set the media list entry to.
    /// </summary>
    public EMediaListStatus Status { get; }

    /// <summary>
    /// The progress to set the media list entry to.
    /// </summary>
    public int Progress { get; }

    /// <summary>
    /// The username of the user to update the media list entry for.
    /// </summary>
    public string Username { get; }

    internal UpdateMediaListEntryContext(int providerMediaId, EMediaFormat format, EMediaListStatus status, int progress, string username)
    {
        ProviderMediaId = providerMediaId;
        Format = format;
        Status = status;
        Progress = progress;
        Username = username;
    }

    internal UpdateMediaListEntryContext(int mediaListEntryId, EMediaListStatus status, int progress, string username)
    {
        MediaListEntryId = mediaListEntryId;
        Format = default;
        Status = status;
        Progress = progress;
        Username = username;
    }

    private protected override void LogErrorImpl(ILogger logger, Exception? e)
    {
        if (MediaListEntryId > 0)
        {
            logger.LogError(e, "Unable to update media list entry {MediaListEntryId} in user {Username}'s media list.", MediaListEntryId, Username);
        }
        else
        {
            logger.LogError(e, "Unable to update media list entry for media {MediaId} in user {Username}'s media list.", ProviderMediaId, Username);
        }
    }
}
