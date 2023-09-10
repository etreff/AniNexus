namespace AniNexus;

/// <summary>
/// A service that notifies the developer of certain issues.
/// </summary>
public interface IDeveloperNotificationService
{
    /// <summary>
    /// Sends a message to the developer.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    ValueTask<bool> SendMessageAsync(string message, CancellationToken cancellationToken);

    /// <summary>
    /// Sends an error message to the developer.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    ValueTask<bool> SendErrorAsync(string message, CancellationToken cancellationToken)
    {
        return SendErrorAsync(message, null, cancellationToken);
    }

    /// <summary>
    /// Sends an error message to the developer.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="e">The exception.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    ValueTask<bool> SendErrorAsync(string message, Exception? e, CancellationToken cancellationToken);
}
