using System;

namespace AniNexus.Models.User;

public record UserDTO(
    Guid Id,
    string Username,
    bool TwoFactorEnabled,
    bool IsBanned,
    DateTime? BannedUntil);
