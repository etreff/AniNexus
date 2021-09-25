namespace AniNexus.Models.User;

/// <summary>
/// The underlying login result code.
/// </summary>
public enum ELoginResult : byte
{
    GenericFailure,
    Success,
    InvalidCredentials,
    UserBanned,
    MFACodeRequired,
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
