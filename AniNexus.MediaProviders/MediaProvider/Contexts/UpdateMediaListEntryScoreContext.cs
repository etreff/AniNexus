using Microsoft.Extensions.Logging;

namespace AniNexus.MediaProviders.MediaProvider.Contexts;

/// <summary>
/// The context for updating a media list entry's score in a media provider.
/// </summary>
public sealed class UpdateMediaListEntryScoreContext : ContextBase
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
    /// The value to set the score to.
    /// </summary>
    public MediaScore Score { get; }

    /// <summary>
    /// The username of the user to update the media list entry for.
    /// </summary>
    public string Username { get; }

    internal UpdateMediaListEntryScoreContext(int providerMediaId, EMediaFormat format, MediaScore score, string username)
    {
        ProviderMediaId = providerMediaId;
        Format = format;
        Score = score;
        Username = username;
    }

    internal UpdateMediaListEntryScoreContext(int mediaListEntryId, MediaScore score, string username)
    {
        MediaListEntryId = mediaListEntryId;
        Format = default;
        Score = score;
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
