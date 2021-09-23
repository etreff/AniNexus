using Microsoft.Extensions.Logging;

namespace AniNexus;

/// <summary>
/// Logger event Ids to be passed into <see cref="LoggerMessageAttribute"/>
/// </summary>
public static class LoggerEvents
{
    /// <summary>
    /// A user's MFA is being disabled.
    /// </summary>
    public const int MFADisabling = 40;

    /// <summary>
    /// A user's JWT is being generated.
    /// </summary>
    public const int JWTGenerating = 45;
}

