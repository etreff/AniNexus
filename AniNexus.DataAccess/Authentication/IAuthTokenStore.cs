namespace AniNexus.Authentication;

/// <summary>
/// Defines a service that stores authentication tokens.
/// </summary>
public interface IAuthTokenStore
{
    /// <summary>
    /// Gets an access token from the backing data store.
    /// </summary>
    /// <param name="name">The name of the access token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The access token if found, <see langword="null"/> otherwise.</returns>
    Task<AccessToken?> GetAccessTokenAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Removes the access token with the specified name.
    /// </summary>
    /// <param name="name">The name of the access token to remove.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task ClearAccessTokenAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Saves the access token to the backing store.
    /// </summary>
    /// <param name="accessToken">The access token to save.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SaveAccessTokenAsync(AccessToken accessToken, CancellationToken cancellationToken);
}
