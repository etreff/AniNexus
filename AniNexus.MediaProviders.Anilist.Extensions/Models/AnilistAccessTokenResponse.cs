using System.Text.Json.Serialization;

namespace AniNexus.MediaProviders.Anilist.Models;

/// <summary>
/// The response payload to Anilist's OAuth2 token endpoint.
/// </summary>
internal sealed class AnilistAccessTokenResponse
{
    /// <summary>
    /// The token type.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = default!;

    /// <summary>
    /// The number of seconds that the access token will be valid.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn
    {
        get => _expiresIn;
        set
        {
            _expiresIn = value;
            Expires = DateTime.UtcNow.AddSeconds(value);
        }
    }

    private int _expiresIn;

    /// <summary>
    /// The UTC time in which the access token will expire.
    /// </summary>
    public DateTime Expires { get; private set; }

    /// <summary>
    /// The access token.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = default!;

    /// <summary>
    /// The refresh token.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = default!;

    /// <summary>
    /// Gets the amount of time until the token expires.
    /// </summary>
    public TimeSpan GetExpiryTime()
    {
        return Expires - DateTime.Now;
    }
}
