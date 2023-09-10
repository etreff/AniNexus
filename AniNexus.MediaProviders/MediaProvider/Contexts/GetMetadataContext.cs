using Microsoft.Extensions.Logging;

namespace AniNexus.MediaProviders.MediaProvider.Contexts;

/// <summary>
/// The context for getting a media's metadata from a media provider.
/// </summary>
public sealed class GetMetadataContext : ContextBase
{
    /// <summary>
    /// The Id that the provider has assigned the media.
    /// </summary>
    public int ProviderMediaId { get; }

    /// <summary>
    /// The format of the requested <see cref="ProviderMediaId"/>.
    /// </summary>
    public EMediaFormat Format { get; }

    internal GetMetadataContext(int providerMediaId, EMediaFormat format)
    {
        ProviderMediaId = providerMediaId;
        Format = format;
    }

    private protected override void LogErrorImpl(ILogger logger, Exception? e)
    {
        logger.LogError(e, "Unable to get metadata for media {MediaId}.", ProviderMediaId);
    }
}
