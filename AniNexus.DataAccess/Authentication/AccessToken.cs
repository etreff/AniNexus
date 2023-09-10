namespace AniNexus.Authentication;

/// <summary>
/// Represents an access token.
/// </summary>
public sealed class AccessToken
{
    /// <summary>
    /// The name of the token.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The time the token was issued.
    /// </summary>
    public required DateTime IssuedAt { get; set; }

    /// <summary>
    /// The time the token expires.
    /// </summary>
    public required DateTime ExpiresAt { get; set; }

    /// <summary>
    /// The token type.
    /// </summary>
    public required string TokenType { get; set; }

    /// <summary>
    /// The token.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// The token.
    /// </summary>
    public string? RefreshToken { get; set; }
}
