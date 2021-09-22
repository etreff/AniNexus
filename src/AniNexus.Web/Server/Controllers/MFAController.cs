using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using AniNexus.Infrastructure;
using AniNexus.Models.User;
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AniNexus.Web.Server.Controllers
{
    [Route("/api/account/mfa")]
    public partial class MFAController : Controller
    {
        private readonly IRepositoryProvider RepositoryProvider;
        private readonly ILogger Logger;

        public MFAController(IRepositoryProvider repositoryProvider, ILogger<MFAController> logger)
        {
            RepositoryProvider = repositoryProvider;
            Logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Validate(string username, string? code, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return GetActionResult(false);
            }

            var userRepsitory = RepositoryProvider.GetUserRepository();

            var user = await userRepsitory.GetUserByNameAsync(username, cancellationToken);
            if (user is null)
            {
                return GetActionResult(false);
            }

            if (!user.TwoFactorEnabled)
            {
                return GetActionResult(true);
            }

            string? key = await userRepsitory.GetMFAKeyAsync(user.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(code))
            {
                return GetActionResult(false);
            }

            var mfa = new TwoFactorAuthenticator();
            bool isValid = mfa.ValidateTwoFactorPIN(key, code);

            return GetActionResult(isValid);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Setup(CancellationToken cancellationToken)
        {
            var userRepsitory = RepositoryProvider.GetUserRepository();
            var user = await userRepsitory.GetUserByNameAsync(User.FindFirstValue(ClaimTypes.Name)!, cancellationToken);
            if (user is null)
            {
                return GetActionResult(false);
            }

            byte[]? keyBytes = new byte[40];
            RandomNumberGenerator.Create().GetBytes(keyBytes);

            string key = Convert.ToBase64String(keyBytes);

            var mfa = new TwoFactorAuthenticator();
            var setupInfo = mfa.GenerateSetupCode("AniNexus", user.Username, key, false, 3);

            await userRepsitory.SetMFAKeyAsync(user.Id, key, cancellationToken);

            return Ok(new
            {
                ManualCode = setupInfo.ManualEntryKey,
                QrCode = setupInfo.QrCodeSetupImageUrl
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> VerifySetup(string code, CancellationToken cancellationToken)
        {
            var userRepsitory = RepositoryProvider.GetUserRepository();
            var user = await userRepsitory.GetUserByNameAsync(User.FindFirstValue(ClaimTypes.Name)!, cancellationToken);
            if (user is null)
            {
                return GetActionResult(false);
            }

            string? key = await userRepsitory.GetMFAKeyAsync(user.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(key))
            {
                return GetActionResult(false);
            }

            var mfa = new TwoFactorAuthenticator();
            bool isValid = mfa.ValidateTwoFactorPIN(key, code);
            if (isValid)
            {
                await userRepsitory.SetMFAEnabledAsync(user.Id, cancellationToken);
            }

            return GetActionResult(isValid);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Disable(string code, CancellationToken cancellationToken)
        {
            var userRepsitory = RepositoryProvider.GetUserRepository();
            var user = await userRepsitory.GetUserByNameAsync(User.FindFirstValue(ClaimTypes.Name)!, cancellationToken);
            if (user is null)
            {
                return GetActionResult(false);
            }

            string? key = await userRepsitory.GetMFAKeyAsync(user.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(key))
            {
                return GetActionResult(true);
            }

            var mfa = new TwoFactorAuthenticator();

            bool isValid = mfa.ValidateTwoFactorPIN(key, code);
            return await DisableAsync(user, userRepsitory, isValid, cancellationToken);
        }

        [HttpPost]
        [Authorize(Policy.User.UpdateInfo)]
        public async Task<IActionResult> Disable(Guid userId, CancellationToken cancellationToken)
        {
            var userRepsitory = RepositoryProvider.GetUserRepository();
            var user = await userRepsitory.GetUserByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return GetActionResult(false);
            }

            return await DisableAsync(user, userRepsitory, true, cancellationToken);
        }

        private async Task<IActionResult> DisableAsync(UserDTO user, IUserRepository userRepository, bool isValid, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                LogMFADisabled(Logger, user.Username);
                await userRepository.ClearMFAAsync(user.Id, cancellationToken);
            }

            return GetActionResult(isValid);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IActionResult GetActionResult(bool isValid)
        {
            return isValid
                ? Accepted()
                : UnprocessableEntity();
        }

        [LoggerMessage(EventId = LoggerEvents.MFADisabling, Level = LogLevel.Information, Message = "Sending MFA Disable request for user {Username}")]
        static partial void LogMFADisabled(ILogger logger, string username);
    }
}
