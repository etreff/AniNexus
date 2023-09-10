using Microsoft.Extensions.Logging;

namespace AniNexus.MediaProviders.MediaProvider.Contexts;

/// <summary>
/// The context for searching for a media's metadata by name from a media provider.
/// </summary>
public sealed class SearchMetadataContext : ContextBase
{
    /// <summary>
    /// The name to search for.
    /// </summary>
    public string MediaName { get; }

    /// <summary>
    /// The format to filter by.
    /// </summary>
    public EMediaFormat? Format { get; }

    internal SearchMetadataContext(string mediaName, EMediaFormat? format)
    {
        MediaName = mediaName;
        Format = format;
    }

    private protected override void LogErrorImpl(ILogger logger, Exception? e)
    {
        logger.LogError(e, "Unable to search for metadata for the media {MediaName}.", MediaName);
    }
}
