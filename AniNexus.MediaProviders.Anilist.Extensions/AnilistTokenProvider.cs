using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using AniNexus.Authentication;
using AniNexus.MediaProviders.Anilist.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AniNexus.MediaProviders.Anilist;

internal sealed class AnilistTokenProvider : AccessTokenProvider
{
    protected override string Name { get; } = "Anilist Access Token Provider";

    protected override string AuthTokenName { get; } = "Anilist";

    private readonly HttpClient _httpClient;
    private readonly AnilistOptions _options;

    public AnilistTokenProvider(
        IAuthTokenStore authTokenStore,
        IDeveloperNotificationService developerNotificationService,
        IHttpClientFactory httpClientFactory,
        IOptions<AnilistOptions> options,
        ILogger logger)
        : base(authTokenStore, developerNotificationService, logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _options = options.Value;
    }

    public override void ConfigureHttpClient(HttpClient client)
    {
        client.BaseAddress = new Uri("https://graphql.anilist.co/");
        if (AccessToken is not null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken.Token);
        }
    }

    public override Uri GetApiAccessRequestUri()
    {
        return new Uri($"https://anilist.co/api/v2/oauth/authorize?client_id={_options.ClientId}&redirect_uri={HttpUtility.UrlEncode(_options.RedirectUri)}&response_type=code");
    }

    protected override Task<AccessToken?> FetchAccessTokenAsync(string code, CancellationToken cancellationToken)
    {
        object payload = new
        {
            grant_type = "authorization_code",
            client_id = _options.ClientId,
            client_secret = _options.ClientSecret,
            redirect_uri = _options.RedirectUri,
            code = code
        };

        return GetAccessTokenAsync(payload, cancellationToken);
    }

    protected override Task<AccessToken?> RefreshAccessTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        object payload = new
        {
            grant_type = "refresh_token",
            client_id = _options.ClientId,
            client_secret = _options.ClientSecret,
            refresh_token = refreshToken
        };

        return GetAccessTokenAsync(payload, cancellationToken);
    }

    private async Task<AccessToken?> GetAccessTokenAsync(object payload, CancellationToken cancellationToken)
    {
        using var refreshResponse = await _httpClient.PostAsJsonAsync("https://anilist.co/api/v2/oauth/token", payload, cancellationToken);

        if (!refreshResponse.IsSuccessStatusCode)
        {
            throw new Exception($"Status code {refreshResponse.StatusCode} - {await refreshResponse.Content.ReadAsStringAsync()}");
        }

        var accessToken = await refreshResponse.Content.ReadFromJsonAsync<AnilistAccessTokenResponse>((JsonSerializerOptions?)null, cancellationToken) ?? throw new Exception("Unable to parse access token payload.");
        var now = DateTime.UtcNow;

        return new AccessToken
        {
            ExpiresAt = now.AddSeconds(accessToken.ExpiresIn),
            IssuedAt = now,
            Name = AuthTokenName,
            RefreshToken = accessToken.RefreshToken,
            Token = accessToken.AccessToken,
            TokenType = accessToken.TokenType
        };
    }
}
