using AniNexus.Models.User;

namespace AniNexus.Data.Models
{
    /// <summary>
    /// Login result.
    /// </summary>
    public sealed class LoginResult
    {
        /// <summary>
        /// The result code.
        /// </summary>
        public ELoginResult Code { get; }

        /// <summary>
        /// The user information.
        /// </summary>
        public UserDTO? User { get; init; }

        /// <summary>
        /// The error, if one occurred.
        /// </summary>
        public string? Error { get; init; }

        public LoginResult(ELoginResult code)
        {
            Code = code;
        }

        public LoginResponseDTO ToLoginResponse()
        {
            return new LoginResponseDTO
            {
                Error = Error,
                Code = Code,
                User = User
            };
        }
    }
}
