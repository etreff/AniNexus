using Microsoft.Extensions.Logging;

namespace AniNexus.MediaProviders.MediaProvider.Contexts;

/// <summary>
/// The context for deleting a media list entry from a media provider.
/// </summary>
public sealed class DeleteMediaListEntryContext : ContextBase
{
    /// <summary>
    /// The Id that the provider has assigned the media.
    /// </summary>
    public int ProviderMediaId { get; }

    /// <summary>
    /// The Id of the media list entry.
    /// </summary>
    public int MediaListEntryId { get; }

    /// <summary>
    /// The format of the requested <see cref="ProviderMediaId"/>.
    /// </summary>
    public EMediaFormat Format { get; }

    /// <summary>
    /// The username of the user to delete the media list entry from.
    /// </summary>
    public string Username { get; }

    internal DeleteMediaListEntryContext(int providerMediaId, EMediaFormat format, string username)
    {
        ProviderMediaId = providerMediaId;
        Format = format;
        Username = username;
    }

    internal DeleteMediaListEntryContext(int mediaListEntryId, string username)
    {
        MediaListEntryId = mediaListEntryId;
        Format = default;
        Username = username;
    }

    private protected override void LogErrorImpl(ILogger logger, Exception? e)
    {
        if (MediaListEntryId > 0)
        {
            logger.LogError(e, "Unable to delete media list entry {MediaListEntryId} from user {Username}'s media list.", MediaListEntryId, Username);
        }
        else
        {
            logger.LogError(e, "Unable to delete media list entry for media {MediaId} from user {Username}'s media list.", ProviderMediaId, Username);
        }
    }
}
