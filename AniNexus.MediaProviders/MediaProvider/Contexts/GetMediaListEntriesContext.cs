using Microsoft.Extensions.Logging;

namespace AniNexus.MediaProviders.MediaProvider.Contexts;

/// <summary>
/// The context for getting media list entries from a media provider.
/// </summary>
public sealed class GetMediaListEntriesContext : ContextBase
{
    /// <summary>
    /// The Id that the provider has assigned the media.
    /// </summary>
    public IEnumerable<int>? ProviderMediaIds { get; }

    /// <summary>
    /// The format of the requested <see cref="ProviderMediaIds"/>.
    /// </summary>
    public EMediaFormat Format { get; }

    /// <summary>
    /// The username of the user to get the media list entry from.
    /// </summary>
    public string Username { get; }

    internal GetMediaListEntriesContext(IEnumerable<int>? providerMediaIds, EMediaFormat format, string username)
    {
        ProviderMediaIds = providerMediaIds;
        Format = format;
        Username = username;
    }

    private protected override void LogErrorImpl(ILogger logger, Exception? e)
    {
        logger.LogError(e, "Unable to add get list entries from user {Username}'s media list.", Username);
    }
}
