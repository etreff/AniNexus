namespace AniNexus.Models.User;

/// <summary>
/// The underlying login result code.
/// </summary>
public enum ELoginResult : byte
{
    /// <summary>
    /// The login failed with a generic failure.
    /// </summary>
    GenericFailure,

    /// <summary>
    /// The login was successful.
    /// </summary>
    Success,

    /// <summary>
    /// The logic failed due to invalid credentials.
    /// </summary>
    InvalidCredentials,

    /// <summary>
    /// The login failed because the user is banned.
    /// </summary>
    UserBanned,

    /// <summary>
    /// The login failed because a MFA code is required.
    /// </summary>
    MFACodeRequired,

    /// <summary>
    /// The login failed because the MFA code provided is incorrect.
    /// </summary>
    InvalidMFACode
}

/// <summary>
/// Login result.
/// </summary>
public class LoginResponseDTO
{
    /// <summary>
    /// The result code.
    /// </summary>
    public ELoginResult Code { get; set; }

    /// <summary>
    /// The user information.
    /// </summary>
    public UserDTO? User { get; set; }

    /// <summary>
    /// The error, if one occurred.
    /// </summary>
    public string? Error { get; set; }
}
