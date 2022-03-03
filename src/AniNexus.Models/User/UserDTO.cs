using System.Collections.Generic;

namespace AniNexus.Models.User;

/// <summary>
/// Models a user.
/// </summary>
/// <param name="Username">The user's username.</param>
/// <param name="Token">The user's token.</param>
/// <param name="Claims">The user's claims.</param>
public record UserDTO(
    string Username,
    string Token,
    Dictionary<string, string> Claims);
