namespace AniNexus.Authentication;

/// <summary>
/// Defines a service that hashes and verifies passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Generates a password reset token.
    /// </summary>
    string GeneratePasswordResetToken();

    /// <summary>
    /// Hashes a password.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    string HashPassword(string password);

    /// <summary>
    /// Verifies whether the user-provided password matches their hashed password.
    /// </summary>
    /// <param name="hashedPassword">The hashed user password.</param>
    /// <param name="password">The user-provided clear-text password to verify.</param>
    PasswordVerificationResult VerifyPassword(string hashedPassword, string password);
}
