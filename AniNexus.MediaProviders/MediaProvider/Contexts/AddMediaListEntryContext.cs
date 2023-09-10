using Microsoft.Extensions.Logging;

namespace AniNexus.MediaProviders.MediaProvider.Contexts;

/// <summary>
/// The context for adding a media list entry to a media provider.
/// </summary>
public sealed class AddMediaListEntryContext : ContextBase
{
    /// <summary>
    /// The Id that the provider has assigned the media.
    /// </summary>
    public int ProviderMediaId { get; }

    /// <summary>
    /// The format of the requested <see cref="ProviderMediaId"/>.
    /// </summary>
    public EMediaFormat Format { get; }

    /// <summary>
    /// The status to set the created media list entry to.
    /// </summary>
    public EMediaListStatus Status { get; }

    /// <summary>
    /// The username of the user to create the media list entry for.
    /// </summary>
    public string Username { get; }

    internal AddMediaListEntryContext(int providerMediaId, EMediaFormat format, EMediaListStatus status, string username)
    {
        ProviderMediaId = providerMediaId;
        Format = format;
        Status = status;
        Username = username;
    }

    private protected override void LogErrorImpl(ILogger logger, Exception? e)
    {
        logger.LogError(e, "Unable to add media list entry to user {Username}'s media list for media {MediaId}.", Username, ProviderMediaId);
    }
}
