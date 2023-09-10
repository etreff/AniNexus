namespace AniNexus.Authentication;

/// <summary>
/// Defines a service that manages user identity.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Returns whether the user with the specified username requires an OTP to log into their
    /// account.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <remarks>
    /// Sometimes the workflow is that the OTP prompt only shows if the user enters the correct password.
    /// We would rather reveal that the user has MFA set up than the caller has correctly guessed the password.
    /// It also vastly simplifies login implementation logic by not having to track the login state.
    /// The downside is that credentials will need to be sent over the wire twice if MFA is set up.
    /// </remarks>
    Task<bool> RequiresOtpAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to log a user in.
    /// </summary>
    /// <param name="username">The user's username.</param>
    /// <param name="password">The user's password.</param>
    /// <param name="mfa">The user's MFA token.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<LoginResult> LogIn(string username, string password, string? mfa, CancellationToken cancellationToken = default);
}
