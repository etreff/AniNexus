using AniNexus.Models.User;

namespace AniNexus.Models
{
    /// <summary>
    /// Login result.
    /// </summary>
    public sealed class LoginResult
    {
        /// <summary>
        /// Whether the user was successfully logged in.
        /// </summary>
        public bool Succeeded { get; private set; }

        /// <summary>
        /// Whether the user requires MFA before login can succeed.
        /// </summary>
        public bool TwoFactorRequired { get; private set; }

        /// <summary>
        /// The authentication token.
        /// </summary>
        public string? Token { get; private set; }

        /// <summary>
        /// The reason the login failed.
        /// </summary>
        public string? Error { get; private set; }

        private LoginResult()
        {

        }

        internal static LoginResult Success(string token)
        {
            return new LoginResult
            {
                Succeeded = true,
                Token = token
            };
        }

        public static LoginResult Failed(string? reason = null)
        {
            return new LoginResult
            {
                Succeeded = false,
                Error = reason
            };
        }

        internal static LoginResult MFARequired()
        {
            return new LoginResult
            {
                Succeeded = true,
                TwoFactorRequired = true
            };
        }

        public LoginResponseDTO ToLoginResponse()
        {
            return new LoginResponseDTO
            {
                Error = Error,
                Succeeded = Succeeded,
                Token = Token,
                TwoFactorRequired = TwoFactorRequired
            };
        }
    }
}
