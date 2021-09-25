using System.ComponentModel.DataAnnotations;

namespace AniNexus.Models.User;

/// <summary>
/// A model that contains login information.
/// </summary>
public class LoginRequestDTO
{
    /// <summary>
    /// The username.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "The username is required.")]
    public string Username { get; set; } = default!;

    /// <summary>
    /// The password.
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "The password is required.")]
    public string Password { get; set; } = default!;

    /// <summary>
    /// The MFA code.
    /// </summary>
    public string? TwoFactorCode { get; set; }
}
