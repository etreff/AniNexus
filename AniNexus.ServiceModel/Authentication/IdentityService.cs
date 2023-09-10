using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AniNexus.DataAccess;
using AniNexus.DataAccess.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AniNexus.Authentication;

internal sealed class IdentityService : IIdentityService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IOtpProvider _otpProvider;
    private readonly IDbContextFactory<AniNexusDbContext> _dbContextFactory;
    private readonly OAuth2Options _oauthOptions;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(
        IPasswordHasher passwordHasher,
        IOtpProvider otpProvider,
        IDbContextFactory<AniNexusDbContext> dbContextFactory,
        IOptions<OAuth2Options> oAuth2Options,
        ILogger<IdentityService> logger)
    {
        _passwordHasher = passwordHasher;
        _otpProvider = otpProvider;
        _dbContextFactory = dbContextFactory;
        _oauthOptions = oAuth2Options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> RequiresOtpAsync(string username, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var result = await dbContext.Users.Where(x => x.Username == username).Select(x => new { x.MultiFactorEnabled }).FirstOrDefaultAsync(cancellationToken);
        return result?.MultiFactorEnabled == true;
    }

    /// <inheritdoc />
    public async Task<LoginResult> LogIn(string username, string password, string? mfa, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Log in requested for user {Username}.", username);

        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var user = await dbContext.Users.WithSpecification(new UserIdentitySpecification(username, includeBanReasons: true)).FirstOrDefaultAsync(cancellationToken);
        if (user is null)
        {
            return LoginResult.Failed;
        }

        var passwordResult = _passwordHasher.VerifyPassword(user.Password, password);
        if (passwordResult == PasswordVerificationResult.Failed)
        {
            _logger.LogInformation("Failed login attempt for user {Username}.", username);
            //TODO: Increment failed login attempts.
            return LoginResult.Failed;
        }

        if (user.MultiFactorEnabled)
        {
            bool isEmptySecret = false;
            if (string.IsNullOrWhiteSpace(mfa) ||
                (isEmptySecret = string.IsNullOrWhiteSpace(user.MultiFactorAuthSecret)) ||
                !_otpProvider.IsValidCode(user.MultiFactorAuthSecret!, mfa))
            {
                if (isEmptySecret)
                {
                    _logger.LogError("User {Username} has MFA enabled but no stored secret!", username);
                }

                _logger.LogInformation("Failed login attempt for user {Username}.", username);
                return LoginResult.Failed;
            }
        }

        _logger.LogInformation("Login attempt for user {Username} successful.", username);
        if (passwordResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            string oldPassword = user.Password;
            try
            {
                _logger.LogInformation("Rehashing password for user {Username}.", username);
                user.Password = _passwordHasher.HashPassword(password);
                await dbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Password successfully rehashed for user {Username}.", username);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to update user's password.");
                user.Password = oldPassword;
            }
        }

        var now = DateTime.UtcNow;

        var activeBan = user.BanReasons?.OrderBy(static x => x.BannedUntil, new NullableLastComparer<DateTime?>()).LastOrDefault();
        if (activeBan is not null)
        {
            if (!activeBan.BannedUntil.HasValue)
            {
                return LoginResult.BannedUntil(null);
            }

            if (activeBan.BannedUntil > now)
            {
                return LoginResult.BannedUntil(activeBan.BannedUntil.Value);
            }
        }

        var timeSinceEpoch = (DateTime.UnixEpoch - now).Duration();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Aud, _oauthOptions.Audience),
            new Claim(JwtRegisteredClaimNames.AuthTime, now.ToString("o")),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Exp, ((int)timeSinceEpoch.Add(_oauthOptions.Expires).TotalSeconds).ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, ((int)timeSinceEpoch.TotalSeconds).ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, _oauthOptions.Issuer),
            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Nonce, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            // In the future if we allow shared usernames, this would be a username with a discriminator - the unique username
            // that would be displayed to the user that others can use to identify them by.
            //new Claim(JwtRegisteredClaimNames.UniqueName, "")
        };

        if (user.DateOfBirth.HasValue)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Birthdate, user.DateOfBirth.Value.ToString()));
        }

        var identity = new ClaimsIdentity(claims, "Basic");
        string jwtToken = GenerateJwtToken(claims, now);

        return new(identity, jwtToken);
    }

    private string GenerateJwtToken(List<Claim> claims, DateTime now)
    {
        var securityKey = _oauthOptions.GetSecurityKey();
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            _oauthOptions.Issuer,
            _oauthOptions.Audience,
            claims,
            null,
            now.Add(_oauthOptions.Expires),
            credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
