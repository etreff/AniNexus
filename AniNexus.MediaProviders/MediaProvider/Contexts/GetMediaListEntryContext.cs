using Microsoft.Extensions.Logging;

namespace AniNexus.MediaProviders.MediaProvider.Contexts;

/// <summary>
/// The context for getting a media list entry from a media provider.
/// </summary>
public sealed class GetMediaListEntryContext : ContextBase
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
    /// The username of the user to get the media list entry from.
    /// </summary>
    public string Username { get; }

    internal GetMediaListEntryContext(int providerMediaId, EMediaFormat format, string username)
    {
        ProviderMediaId = providerMediaId;
        Format = format;
        Username = username;
    }

    private protected override void LogErrorImpl(ILogger logger, Exception? e)
    {
        logger.LogError(e, "Unable to add get list entry from user {Username}'s media list for media {MediaId}.", Username, ProviderMediaId);
    }
}
