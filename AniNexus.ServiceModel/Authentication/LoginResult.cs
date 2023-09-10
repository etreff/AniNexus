using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace AniNexus.Authentication;

/// <summary>
/// The result of a login operation.
/// </summary>
public class LoginResult
{
    /// <summary>
    /// A failed login result.
    /// </summary>
    public static LoginResult Failed { get; } = new("The username or password was incorrect.");

    /// <summary>
    /// Whether the login attempt was successful.
    /// </summary>
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    [MemberNotNullWhen(true, nameof(Identity), nameof(JwtToken))]
    public bool IsSuccess { get; }

    /// <summary>
    /// The error message associated with the failure.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// The identity of the logged-in user.
    /// </summary>
    public IIdentity? Identity { get; }

    /// <summary>
    /// The JWT token for the login.
    /// </summary>
    public string? JwtToken { get; }

    internal LoginResult(IIdentity identity, string jwtToken)
    {
        Identity = identity;
        JwtToken = jwtToken;
    }

    internal LoginResult(string errorMessage)
    {
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }



    /// <summary>
    /// Creates a failed <see cref="LoginResult"/>
    /// </summary>
    /// <param name="time">The time until which the user is banned.</param>
    public static LoginResult BannedUntil(DateTime? time)
    {
        return time.HasValue
            ? new("The user is banned.")
            : new($"The user is banned until {time!.Value:F}.");
    }
}
