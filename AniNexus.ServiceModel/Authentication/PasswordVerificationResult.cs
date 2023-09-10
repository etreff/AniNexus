namespace AniNexus.Authentication;

/// <summary>
/// Defines the result of a password verification operation.
/// </summary>
public enum PasswordVerificationResult
{
    /// <summary>
    /// The password did not match.
    /// </summary>
    Failed,

    /// <summary>
    /// The password matched.
    /// </summary>
    Success,

    /// <summary>
    /// The password matched, but due to a change in algorithm it should be rehashed.
    /// </summary>
    SuccessRehashNeeded
}
