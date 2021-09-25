using System.Collections.Generic;

namespace AniNexus.Models.User;

public record UserDTO(
    string Username,
    string Token,
    Dictionary<string, string> Claims);
