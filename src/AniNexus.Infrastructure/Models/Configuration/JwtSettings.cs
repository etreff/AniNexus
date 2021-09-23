namespace AniNexus.Models.Configuration;

/// <summary>
/// JWT settings.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// The key used to sign and verify JWT tokens.
    /// </summary>
    public string Key { get; set; } = default!;

    /// <summary>
    /// The number of minutes to skew the JWT expiration when it is validated.
    /// </summary>
    public int ClockSkew { get; set; }

    /// <summary>
    /// The number of minutes the JWT is valid for.
    /// </summary>
    /// <remarks>
    /// The default is 10,080, or 7 days.
    /// </remarks>
    public int Expires { get; set; } = 10080;

    /// <summary>
    /// The issuer.
    /// </summary>
    public string Issuer { get; set; } = default!;

    /// <summary>
    /// The audience.
    /// </summary>
    public string Audience { get; set; } = default!;
}

