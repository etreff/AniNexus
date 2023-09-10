using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AniNexus.Authentication;

/// <summary>
/// The base class for a background service that manages an API access token.
/// </summary>
public abstract class AccessTokenProvider : BackgroundService
{
    /// <summary>
    /// The name of this provider.
    /// </summary>
    protected abstract string Name { get; }

    /// <summary>
    /// The name of the access token.
    /// </summary>
    protected abstract string AuthTokenName { get; }

    /// <summary>
    /// The logger.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// The authentication token store.
    /// </summary>
    protected IAuthTokenStore AuthTokenStore { get; }

    /// <summary>
    /// The developer notification service.
    /// </summary>
    protected IDeveloperNotificationService DeveloperNotificationService { get; }

    /// <summary>
    /// The active access token for this provider.
    /// </summary>
    protected AccessToken? AccessToken { get; private set; }

    private DateTime _lastDeveloperNotification = DateTime.MinValue;

    /// <summary>
    /// Creates a new <see cref="AccessTokenProvider"/> instance.
    /// </summary>
    /// <param name="authTokenStore">The authentication token store.</param>
    /// <param name="developerNotificationService">The developer notification service.</param>
    /// <param name="logger">The logger.</param>
    protected AccessTokenProvider(IAuthTokenStore authTokenStore, IDeveloperNotificationService developerNotificationService, ILogger logger)
    {
        AuthTokenStore = authTokenStore;
        DeveloperNotificationService = developerNotificationService;
        Logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        Logger.LogInformation("Checking for existing authentication token with the name {TokenName}.", AuthTokenName);

        var accessToken = await AuthTokenStore.GetAccessTokenAsync(AuthTokenName, stoppingToken);
        if (accessToken is not null)
        {
            Logger.LogInformation("Existing authentication token with the name {TokenName} has been found.", AuthTokenName);
            AccessToken = accessToken;

            // Clear it out if it has expired.
            if (accessToken.ExpiresAt <= DateTime.UtcNow)
            {
                Logger.LogInformation("The existing authentication token with the name {TokenName} has expired. Clearing the cached authentication token.", AuthTokenName);
                AccessToken = null;

                await AuthTokenStore.ClearAccessTokenAsync(AuthTokenName, stoppingToken);
            }
        }
        else
        {
            Logger.LogWarning("No existing authentication token with the name {TokenName} was found. The token will need to be manually re-obtained.", AuthTokenName);
            await DeveloperNotificationService.SendErrorAsync($"The access token with the name {AuthTokenName} is not set and no cached version is available. Please visit the authentication page to set it.", stoppingToken);
            _lastDeveloperNotification = DateTime.Now;
        }

        Logger.LogInformation("Media provider {ProviderName} service started.", Name);

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(30));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            // We need to wait until we have an access token. This will be a pain in the ass to make sure we refresh this
            // every time we restart the service, but at the very least maintenance after that will be painless.
            if (AccessToken is null)
            {
                Logger.LogWarning("The authentication token with the name {TokenName} is not set. API calls may fail.", AuthTokenName);

                // Only send a developer notification for this error once every 12 hours.
                if ((_lastDeveloperNotification - DateTime.Now).Duration() > TimeSpan.FromHours(12) &&
                    await DeveloperNotificationService.SendErrorAsync($"The authentication token with the name {AuthTokenName} is not set. Please visit the authentication page to set it.", stoppingToken))
                {
                    _lastDeveloperNotification = DateTime.Now;
                }

                continue;
            }

            if (AccessToken.ExpiresAt - DateTime.UtcNow >= TimeSpan.FromMinutes(2))
            {
                continue;
            }

            Logger.LogInformation("Refreshing authentication token with the name {TokenName}.", AuthTokenName);
            try
            {
                if (!string.IsNullOrWhiteSpace(AccessToken.RefreshToken))
                {
                    var newToken = await RefreshAccessTokenAsync(AccessToken.RefreshToken, stoppingToken);
                    if (newToken is not null)
                    {
                        await AuthTokenStore.SaveAccessTokenAsync(newToken, stoppingToken);
                        Logger.LogInformation("The authentication token with the name {TokenName} was refreshed successfully.", AuthTokenName);
                    }
                }
                else
                {
                    Logger.LogError("The authentication token with the name {TokenName} failed to refresh successfully - no refresh token was available.", AuthTokenName);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, "The authentication token with the name {TokenName} failed to refresh successfully.", AuthTokenName);
            }
        }
    }

    /// <summary>
    /// Configures the <see cref="HttpClient"/> to interact with the API that refreshes or grants an access token.
    /// </summary>
    /// <param name="client">The client to configure.</param>
    public virtual void ConfigureHttpClient(HttpClient client)
    {
    }

    /// <summary>
    /// Requests a new access token using the specified code.
    /// </summary>
    /// <param name="code">The access code given to the user to grant tokens on the user's behalf.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Whether the token was granted successfully.</returns>
    public async Task<bool> GrantAccessTokenAsync(string code, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Fetching new access token for token with name {TokenName}.", AuthTokenName);
        var newToken = await FetchAccessTokenAsync(code, cancellationToken);
        if (newToken is not null)
        {
            try
            {
                Logger.LogInformation("Saving access token with name {TokenName}.", AuthTokenName);
                await AuthTokenStore.ClearAccessTokenAsync(AuthTokenName, cancellationToken);
                await AuthTokenStore.SaveAccessTokenAsync(newToken, cancellationToken);

                Logger.LogInformation("Access token {TokenName} saved successfully.", AuthTokenName);
                AccessToken = newToken;

                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to save access token with name {TokenName}.", AuthTokenName);
                return false;
            }
        }

        Logger.LogError("Failed to fetch access token with name {TokenName}.", AuthTokenName);
        return false;
    }

    /// <summary>
    /// Requests a new access token using the specified code.
    /// </summary>
    /// <param name="code">The access code given to the user to grant tokens on the user's behalf.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The fetched access token on success, <see langword="null"/> otherwise.</returns>
    protected abstract Task<AccessToken?> FetchAccessTokenAsync(string code, CancellationToken cancellationToken);

    /// <summary>
    /// Requests a new access token using the specified refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token that can be used to renew an existing access token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The new access token on success, <see langword="null"/> otherwise.</returns>
    protected abstract Task<AccessToken?> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the URI that the user must go to in order to grant permission for the application to act on their behalf.
    /// </summary>
    public abstract Uri GetApiAccessRequestUri();
}
