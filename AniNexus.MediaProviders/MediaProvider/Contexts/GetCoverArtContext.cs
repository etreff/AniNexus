using Microsoft.Extensions.Logging;

namespace AniNexus.MediaProviders.MediaProvider.Contexts;

/// <summary>
/// The context for getting a media's cover art from a media provider.
/// </summary>
public sealed class GetCoverArtContext : ContextBase
{
    /// <summary>
    /// The Id that the provider has assigned the media.
    /// </summary>
    public int ProviderMediaId { get; }

    /// <summary>
    /// The format of the requested <see cref="ProviderMediaId"/>.
    /// </summary>
    public EMediaFormat Format { get; }

    internal GetCoverArtContext(int providerMediaId, EMediaFormat format)
    {
        ProviderMediaId = providerMediaId;
        Format = format;
    }

    private protected override void LogErrorImpl(ILogger logger, Exception? e)
    {
        logger.LogError(e, "Unable to get cover art for media {MediaId}.", ProviderMediaId);
    }
}
