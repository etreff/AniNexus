namespace AniNexus.DataAccess.Entities;

/// <summary>
/// Models an AniNexus user's basic identifying information.
/// </summary>
public sealed class UserEntity : Entity<UserEntity>
{
    /// <summary>
    /// The user's username.
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// The user's password.
    /// </summary>
    /// <remarks>
    /// If the user has not set their password it will be set to a randomly
    /// generated password until they set their own.
    /// </remarks>
    public required string Password { get; set; }

    /// <summary>
    /// The user's date of birth.
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// The user's email address.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// The code that was sent to the user to change/set their password.
    /// </summary>
    public string? PasswordResetCode { get; set; }

    /// <summary>
    /// Whether MFA is enabled.
    /// </summary>
    public bool MultiFactorEnabled { get; set; }

    /// <summary>
    /// The MFA secret code.
    /// </summary>
    public string? MultiFactorAuthSecret { get; set; }

    /// <summary>
    /// The teams that the user belongs to.
    /// </summary>
    public IList<AuthTeamUserMapEntity>? Teams { get; set; }

    /// <summary>
    /// Reasons why a user was banned.
    /// </summary>
    public IList<UserBannedEntity>? BanReasons { get; set; }
}
