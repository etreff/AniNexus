using System.Security.Claims;
using System.Security.Cryptography;
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
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO model, CancellationToken cancellationToken)
        {
            if (model is null || !ModelState.IsValid)
            {
                return UnprocessableEntity(new LoginResponseDTO
                {
                    Code = ELoginResult.InvalidCredentials,
                    Error = "The login request was invalid."
                });
            }

            await using var scope = RepositoryProvider.CreateAsyncScope();
            var userRepository = scope.GetUserRepository();
            var loginResult = await userRepository.LoginAsync(model.Username, model.Password, model.TwoFactorCode, cancellationToken);

            return loginResult.Code switch
            {
                ELoginResult.Success => Ok(loginResult.ToLoginResponse()),
                ELoginResult.UserBanned => Unauthorized(loginResult.ToLoginResponse()),
                _ => UnprocessableEntity(loginResult.ToLoginResponse())
            };
        }

        [HttpPost]
        [Route("mfa/setup")]
        [Authorize]
        public async Task<IActionResult> Setup(CancellationToken cancellationToken)
        {
            await using var scope = RepositoryProvider.CreateAsyncScope(cancellationToken);
            var userRepository = scope.GetUserRepository();

            var user = await userRepository.GetUserByNameAsync(User.FindFirstValue(ClaimTypes.Name)!, cancellationToken);
            if (user is null)
            {
                return UnprocessableEntity();
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
        [Route("mfa/verify")]
        [Authorize]
        public async Task<IActionResult> VerifySetup(string code, CancellationToken cancellationToken)
        {
            await using var scope = RepositoryProvider.CreateAsyncScope();
            var userRepository = scope.GetUserRepository();

            var user = await userRepository.GetUserByNameAsync(User.FindFirstValue(ClaimTypes.Name)!, cancellationToken);
            if (user is null)
            {
                return UnprocessableEntity();
            }

            string? key = await userRepository.GetMFAKeyAsync(user.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(key))
            {
                return UnprocessableEntity();
            }

            var mfa = new TwoFactorAuthenticator();
            bool isValid = mfa.ValidateTwoFactorPIN(key, code);
            if (isValid)
            {
                await userRepository.SetMFAEnabledAsync(user.Id, cancellationToken);
            }

            return isValid
                ? Ok()
                : UnprocessableEntity();
        }

        [HttpPost]
        [Route("mfa/disable")]
        [Authorize]
        public async Task<IActionResult> Disable(string code, CancellationToken cancellationToken)
        {
            await using var scope = RepositoryProvider.CreateAsyncScope();
            var userRepository = scope.GetUserRepository();

            var user = await userRepository.GetUserByNameAsync(User.FindFirstValue(ClaimTypes.Name)!, cancellationToken);
            if (user is null)
            {
                return UnprocessableEntity();
            }

            string? key = await userRepository.GetMFAKeyAsync(user.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(key))
            {
                return UnprocessableEntity();
            }

            var mfa = new TwoFactorAuthenticator();

            bool isValid = mfa.ValidateTwoFactorPIN(key, code);
            return await DisableAsync(user, userRepository, isValid, cancellationToken);
        }

        [HttpPost]
        [Route("mfa/disable-by-id")]
        [Authorize(Policy.User.UpdateInfo)]
        public async Task<IActionResult> Disable(Guid userId, CancellationToken cancellationToken)
        {
            await using var scope = RepositoryProvider.CreateAsyncScope();
            var userRepository = scope.GetUserRepository();

            var user = await userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (user is null)
            {
                return UnprocessableEntity();
            }

            return await DisableAsync(user, userRepository, true, cancellationToken);
        }

        private async Task<IActionResult> DisableAsync(UserInfo user, IUserRepository userRepository, bool isValid, CancellationToken cancellationToken)
        {
            if (isValid)
            {
                LogMFADisabled(Logger, user.Username);
                await userRepository.ClearMFAAsync(user.Id, cancellationToken);
            }

            return isValid
                ? Ok()
                : UnprocessableEntity();
        }

        [LoggerMessage(EventId = LoggerEvents.MFADisabling, Level = LogLevel.Information, Message = "Sending MFA Disable request for user {Username}")]
        static partial void LogMFADisabled(ILogger logger, string username);
    }
}
