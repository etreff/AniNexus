namespace AniNexus.Models.User;

/// <summary>
/// Login result.
/// </summary>
public class LoginResponseDTO
{
    /// <summary>
    /// Whether the user was successfully logged in.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Whether the user requires MFA before login can succeed.
    /// </summary>
    public bool TwoFactorRequired { get; set; }

    /// <summary>
    /// The authentication token.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// The reason the login failed.
    /// </summary>
    public string? Error { get; set; }
}
