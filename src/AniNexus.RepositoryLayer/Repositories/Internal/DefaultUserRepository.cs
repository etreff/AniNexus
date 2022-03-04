using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AniNexus.Data;
using AniNexus.Data.Models;
using AniNexus.Data.Models.Configuration;
using AniNexus.Data.Models.User;
using AniNexus.Data.Repository;
using AniNexus.Data.Repository.Internal;
using AniNexus.Domain;
using AniNexus.Domain.Models;
using AniNexus.Models.User;
using Google.Authenticator;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AniNexus.Repository.Internal
{
    internal partial class DefaultUserRepository : DefaultRepositoryBase, IUserRepository
    {
        private readonly IOptions<JwtSettings> JwtSettings;

        public DefaultUserRepository(ApplicationDbContext dbContext, IOptions<JwtSettings> jwtSettings, ILogger<DefaultUserRepository> logger)
            : base(dbContext, logger)
        {
            JwtSettings = jwtSettings;
        }

        public async Task ClearMFAAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await Context.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user is null)
            {
                return;
            }

            LogMFADisabled(Logger, user.Username);

            user.TwoFactorEnabled = false;
            user.TwoFactorKey = null;
        }

        public async Task<UserInfo> ClearMFAAsync(UserInfo user, CancellationToken cancellationToken)
        {
            var userModel = await Context.Users.FindAsync(new object[] { user.Id }, cancellationToken);
            if (userModel is null)
            {
                throw new InvalidOperationException("The specified user does not exist.");
            }

            LogMFADisabled(Logger, userModel.Username);

            userModel.TwoFactorEnabled = false;
            userModel.TwoFactorKey = null;

            return MapUser(userModel);
        }

        public async Task<string?> GetMFAKeyAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user is null)
            {
                return null;
            }

            return user.TwoFactorKey;
        }

        public async Task<string?> GetMFAKeyAsync(string username, CancellationToken cancellationToken)
        {
            var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
            if (user is null)
            {
                return null;
            }

            return user.TwoFactorKey;
        }

        public async Task<UserInfo?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            return MapUser(user);
        }

        public async Task<UserInfo?> GetUserByNameAsync(string username, CancellationToken cancellationToken)
        {
            var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

            return MapUser(user);
        }

        public async Task<UserInfo> SetMFAEnabledAsync(UserInfo user, CancellationToken cancellationToken)
        {
            var userModel = await Context.Users.FindAsync(new object[] { user.Id }, cancellationToken);
            if (userModel is null)
            {
                throw new InvalidOperationException("The specified user does not exist.");
            }

            userModel.TwoFactorEnabled = true;
            return MapUser(userModel);
        }

        public async Task SetMFAEnabledAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await Context.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user is null)
            {
                return;
            }

            user.TwoFactorEnabled = true;
        }


        public async Task SetMFAKeyAsync(Guid userId, string key, CancellationToken cancellationToken)
        {
            var user = await Context.Users.FindAsync(new object[] { userId }, cancellationToken);
            if (user is null)
            {
                return;
            }

            user.TwoFactorKey = key;
        }

        private static readonly Func<ApplicationDbContext, string, Task<UserModel?>> GetUserByNameQuery = EF.CompileAsyncQuery((ApplicationDbContext context, string username) =>
            context.Users
                .AsNoTrackingWithIdentityResolution()
                .Include(m => m.Claims)
                .FirstOrDefault(u => u.Username == username));
        public async Task<LoginResult> LoginAsync(string username, string password, string? code, CancellationToken cancellationToken = default)
        {
            var user = await GetUserByNameQuery(Context, username);
            if (user is null)
            {
                return new LoginResult(ELoginResult.InvalidCredentials)
                {
                    Error = "The username or password is incorrect."
                };
            }

            // Do this first since password verification time can cause a token
            // to expire. We want to delay returning this as the failure until after
            // the password has been validated. We do not want to prompt for MFA
            // if the password is incorrect.
            bool requiresMFA = user.TwoFactorKey is not null && user.TwoFactorEnabled;
            bool? isMFAValid = null;
            if (requiresMFA && !string.IsNullOrWhiteSpace(code))
            {
                var mfa = new TwoFactorAuthenticator();
                isMFAValid = mfa.ValidateTwoFactorPIN(user.TwoFactorKey, code);
            }

            if (!Argon2.Verify(user.PasswordHash, password))
            {
                return new LoginResult(ELoginResult.InvalidCredentials)
                {
                    Error = "The username or password is incorrect."
                };
            }

            if (isMFAValid.HasValue)
            {
                if (!isMFAValid.Value)
                {
                    return new LoginResult(ELoginResult.InvalidMFACode)
                    {
                        Error = "The MFA code was invalid."
                    };
                }
            }
            else if (requiresMFA)
            {
                return new LoginResult(ELoginResult.MFACodeRequired)
                {
                    Error = "The user requires an MFA code to supplement login."
                };
            }

            if (user.IsBanned)
            {
                if (!user.BannedUntil.HasValue)
                {
                    return new LoginResult(ELoginResult.UserBanned)
                    {
                        Error = "The user is banned."
                    };
                }

                var now = DateTime.UtcNow;
                if (user.BannedUntil.Value > now)
                {
                    return new LoginResult(ELoginResult.UserBanned)
                    {
                        Error = "The user is banned."
                    };
                }
                else
                {
                    // Go ahead and unban them.
                    user.IsBanned = false;
                    user.BannedUntil = null;
                }
            }

            string token = GetJwtToken(user);
            return new LoginResult(ELoginResult.Success)
            {
                User = new UserDTO(
                    user.Username,
                    GetJwtToken(user),
                    user.Claims.ToDictionary(k => k.Claim.ClaimType, v => v.Claim.ClaimValue))
            };
        }

        private string GetJwtToken(UserModel user)
        {
            LogJWTGenerating(Logger, user.Username);

            byte[] key = Encoding.ASCII.GetBytes(JwtSettings.Value.Key);

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            foreach (var claim in user.Claims.Select(m => m.Claim))
            {
                claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = JwtSettings.Value.Issuer,
                IssuedAt = DateTime.UtcNow,
                Audience = JwtSettings.Value.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(JwtSettings.Value.Expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [return: NotNullIfNotNull("user")]
        private static UserInfo? MapUser(UserModel? user)
        {
            if (user is null)
            {
                return null;
            }

            return new UserInfo(
                user.Id,
                user.Username,
                user.PasswordHash,
                user.TwoFactorEnabled,
                user.TwoFactorKey,
                user.IsBanned,
                user.BannedUntil
            );
        }

        [LoggerMessage(EventId = LoggerEvents.MFADisabling, Level = LogLevel.Information, Message = "Disabling MFA for user {Username}")]
        static partial void LogMFADisabled(ILogger logger, string username);

        [LoggerMessage(EventId = LoggerEvents.JWTGenerating, Level = LogLevel.Information, Message = "Generating JWT for user {Username}")]
        static partial void LogJWTGenerating(ILogger logger, string username);
    }
}
