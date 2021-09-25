using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using AniNexus.Models;
using AniNexus.Models.User;
using AniNexus.Repository;
using Google.Authenticator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AniNexus.Web.Server.Controllers
{
    [Route("/api/account/authenticate")]
    public partial class AuthenticationController : Controller
    {
        private readonly IRepositoryProvider RepositoryProvider;
        private readonly ILogger Logger;

        public AuthenticationController(IRepositoryProvider repositoryProvider, ILogger<AuthenticationController> logger)
        {
            RepositoryProvider = repositoryProvider;
            Logger = logger;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginRequestDTO model, CancellationToken cancellationToken)
        {
            if (model is null || !ModelState.IsValid)
            {
                return Unauthorized(LoginResult.Failed("A username and password is required.").ToLoginResponse());
            }

            await using var scope = RepositoryProvider.CreateAsyncScope();
            var userRepository = scope.GetUserRepository();
            var loginInfo = await userRepository.LoginAsync(model.Username, model.Password, cancellationToken);
            if (!loginInfo.Succeeded)
            {
                return Unauthorized(loginInfo.ToLoginResponse());
            }

            return Ok(loginInfo.ToLoginResponse());
        }

        [HttpPost]
        [Route("/mfa/validate")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateMFA(string username, string? code, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return GetActionResult(false);
            }

            await using var scope = RepositoryProvider.CreateAsyncScope();
            var userRepository = scope.GetUserRepository();

            var user = await userRepository.GetUserByNameAsync(username, cancellationToken);
            if (user is null)
            {
                return GetActionResult(false);
            }

            if (!user.TwoFactorEnabled)
            {
                return GetActionResult(true);
            }

            string? key = await userRepository.GetMFAKeyAsync(user.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(code))
            {
                return GetActionResult(false);
            }

            var mfa = new TwoFactorAuthenticator();
            bool isValid = mfa.ValidateTwoFactorPIN(key, code);

            return GetActionResult(isValid);
        }

        [HttpPost]
        [Route("/mfa/setup")]
        [Authorize]
        public async Task<IActionResult> Setup(CancellationToken cancellationToken)
        {
            await using var scope = RepositoryProvider.CreateAsyncScope();
            var userRepository = scope.GetUserRepository();

            var user = await userRepository.GetUserByNameAsync(User.FindFirstValue(ClaimTypes.Name)!, cancellationToken);
            if (user is null)
            {
                return GetActionResult(false);
            }

            byte[] keyBytes = new byte[40];
            RandomNumberGenerator.Create().GetBytes(keyBytes);

            string key = Convert.ToBase64String(keyBytes);

            var mfa = new TwoFactorAuthenticator();
            var setupInfo = mfa.GenerateSetupCode("AniNexus", user.Username, key, false, 3);

            await userRepository.SetMFAKeyAsync(user.Id, key, cancellationToken);

            return Ok(new
            {
                ManualCode = setupInfo.ManualEntryKey,
                QrCode = setupInfo.QrCodeSetupImageUrl
            });
        }

        [HttpPost]
        [Route("/mfa/verify")]
        [Authorize]
        public async Task<IActionResult> VerifySetup(string code, CancellationToken cancellationToken)
        {
            await using var scope = RepositoryProvider.CreateAsyncScope();
            var userRepository = scope.GetUserRepository();

            var user = await userRepository.GetUserByNameAsync(User.FindFirstValue(ClaimTypes.Name)!, cancellationToken);
            if (user is null)
            {
                return GetActionResult(false);
            }

            string? key = await userRepository.GetMFAKeyAsync(user.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(key))
            {
                return GetActionResult(false);
            }

            var mfa = new TwoFactorAuthenticator();
            bool isValid = mfa.ValidateTwoFactorPIN(key, code);
            if (isValid)
            {
                await userRepository.SetMFAEnabledAsync(user.Id, cancellationToken);
            }

            return GetActionResult(isValid);
        }

        [HttpPost]
        [Route("/mfa/disable")]
        [Authorize]
        public async Task<IActionResult> Disable(string code, CancellationToken cancellationToken)
        {
            await using var scope = RepositoryProvider.CreateAsyncScope();
            var userRepository = scope.GetUserRepository();

            var user = await userRepository.GetUserByNameAsync(User.FindFirstValue(ClaimTypes.Name)!, cancellationToken);
            if (user is null)
            {
                return GetActionResult(false);
            }

            string? key = await userRepository.GetMFAKeyAsync(user.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(key))
            {
                return GetActionResult(true);
            }

            var mfa = new TwoFactorAuthenticator();

            bool isValid = mfa.ValidateTwoFactorPIN(key, code);
            return await DisableAsync(user, userRepository, isValid, cancellationToken);
        }

        [HttpPost]
        [Route("/mfa/disable-by-id")]
        [Authorize(Policy.User.UpdateInfo)]
        public async Task<IActionResult> Disable(Guid userId, CancellationToken cancellationToken)
        {
            await using var scope = RepositoryProvider.CreateAsyncScope();
            var userRepository = scope.GetUserRepository();

            var user = await userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return GetActionResult(false);
            }

            return await DisableAsync(user, userRepository, true, cancellationToken);
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
