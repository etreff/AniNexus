using Microsoft.Extensions.Logging;

namespace AniNexus.MediaProviders.MediaProvider.Contexts;

/// <summary>
/// The base class for a media provider operation context.
/// </summary>
public abstract class ContextBase
{
    /// <summary>
    /// Logs an error message appropriate for this context.
    /// </summary>
    /// <param name="logger">The logger to log to.</param>
    /// <param name="e">The exception.</param>
    public void LogError(ILogger logger, Exception? e = null)
    {
        LogErrorImpl(logger, e);
    }

    /// <summary>
    /// Logs an error message appropriate for this context.
    /// </summary>
    /// <param name="logger">The logger to log to.</param>
    /// <param name="properties">Additional properties to add to the log context.</param>
    /// <param name="e">The exception.</param>
    public void LogError(ILogger logger, Dictionary<string, object?> properties, Exception? e = null)
    {
        properties ??= new Dictionary<string, object?>(0);
        using var scope = logger.BeginScope(properties);
        LogErrorImpl(logger, e);
    }

    private protected abstract void LogErrorImpl(ILogger logger, Exception? e);
}
