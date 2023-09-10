using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AniNexus.Authentication;

/// <summary>
/// The OAuth2.0 options for the application.
/// </summary>
public sealed class OAuth2Options
{
    /// <summary>
    /// The issuer.
    /// </summary>
    public string Issuer { get; set; } = default!;

    /// <summary>
    /// The audience.
    /// </summary>
    public string Audience { get; set; } = default!;

    /// <summary>
    /// The signing key.
    /// </summary>
    public string SigningKey { get; set; } = default!;

    /// <summary>
    /// The amount of time a login is valid for.
    /// </summary>
    public TimeSpan Expires { get; set; } = TimeSpan.FromDays(7);

    private readonly Lazy<SymmetricSecurityKey> _signingKey;

    /// <summary>
    /// Creates a new <see cref="OAuth2Options"/> instance.
    /// </summary>
    public OAuth2Options()
    {
        _signingKey = new(() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)));
    }

    /// <summary>
    /// Gets the signing key.
    /// </summary>
    public SymmetricSecurityKey GetSecurityKey()
    {
        return _signingKey.Value;
    }
}
