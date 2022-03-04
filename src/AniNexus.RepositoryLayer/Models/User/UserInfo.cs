using Isopoh.Cryptography.Argon2;

namespace AniNexus.Data.Models.User;

public record UserInfo(
    Guid Id,
    string Username,
    string PasswordHash,
    bool TwoFactorEnabled,
    string? TwoFactorKey,
    bool IsBanned,
    DateTime? BannedUntil)
{
    /// <summary>
    /// Verifies that the specified password produces the same value as stored
    /// in <see cref="PasswordHash"/>.
    /// </summary>
    /// <param name="password">The plain-text password to verify.</param>
    public bool Verify(string password)
        => Argon2.Verify(PasswordHash, password);
};

