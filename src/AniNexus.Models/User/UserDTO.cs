using System;

namespace AniNexus.Models.User
{
    public class UserDTO
    {
        public Guid Id { get; }

        public string Username { get; }

        public bool TwoFactorEnabled { get; set; }

        public UserDTO(Guid userId, string username)
        {
            Id = userId;
            Username = username;
        }
    }
}
