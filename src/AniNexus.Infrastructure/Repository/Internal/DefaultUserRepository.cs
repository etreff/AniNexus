﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AniNexus.Domain;
using AniNexus.Domain.Models;
using AniNexus.Models;
using AniNexus.Models.Configuration;
using AniNexus.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AniNexus.Repository.Internal;

internal partial class DefaultUserRepository : IUserRepository
{
    private readonly ApplicationDbContext Context;
    private readonly IOptions<JwtSettings> JwtSettings;
    private readonly ILogger Logger;

    public DefaultUserRepository(ApplicationDbContext dbContext, IOptions<JwtSettings> jwtSettings, ILogger<DefaultUserRepository> logger)
    {
        Context = dbContext;
        JwtSettings = jwtSettings;
        Logger = logger;
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

    public async Task<UserDTO?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return MapUser(user);
    }

    public async Task<UserDTO?> GetUserByNameAsync(string username, CancellationToken cancellationToken)
    {
        var user = await Context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

        return MapUser(user);
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

    public async Task<LoginResult> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        string passwordHash = password;
        var user = await Context.Users
            .Include(m => m.Claims)
            .FirstOrDefaultAsync(u => u.Username == username && u.PasswordHash == passwordHash, cancellationToken);
        if (user is null)
        {
            return LoginResult.Failed();
        }

        if (user.IsBanned)
        {
            if (!user.BannedUntil.HasValue)
            {
                return LoginResult.Failed("User is banned.");
            }

            var now = DateTime.UtcNow;
            if (user.BannedUntil.Value > now)
            {
                return LoginResult.Failed("User is banned.");
            }
            else
            {
                user.IsBanned = false;
                user.BannedUntil = null;
            }
        }

        if (user.TwoFactorKey is not null && !user.TwoFactorEnabled)
        {
            return LoginResult.MFARequired();
        }

        string token = GetJwtToken(user);
        return LoginResult.Success(token);
    }

    private string GetJwtToken(UserModel user)
    {
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
    private static UserDTO? MapUser(UserModel? user)
    {
        if (user is null)
        {
            return null;
        }

        return new UserDTO(
            user.Id,
            user.Username,
            user.TwoFactorEnabled,
            user.IsBanned
        );
    }

    [LoggerMessage(EventId = LoggerEvents.MFADisabling, Level = LogLevel.Information, Message = "Disabling MFA for user {Username}")]
    static partial void LogMFADisabled(ILogger logger, string username);

    [LoggerMessage(EventId = LoggerEvents.JWTGenerating, Level = LogLevel.Information, Message = "Generating JWT for user {Username}")]
    static partial void LogJWTGenerating(ILogger logger, string username);
}