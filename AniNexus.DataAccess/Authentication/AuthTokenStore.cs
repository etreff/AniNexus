using AniNexus.DataAccess;
using AniNexus.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AniNexus.Authentication;

internal sealed class AuthTokenStore : IAuthTokenStore
{
    private readonly IDbContextFactory<AniNexusDbContext> _dbContextFactory;
    private readonly ILogger _logger;

    public AuthTokenStore(IDbContextFactory<AniNexusDbContext> dbContextFactory, ILogger<AuthTokenStore> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task ClearAccessTokenAsync(string name, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var set = dbContext.Set<AccessTokenEntity>();
        var token = await set.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        if (token is not null)
        {
            set.Remove(token);

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error has occurred while clearing the cached authentication token with the name {TokenName}.", name);
            }
        }
    }

    public async Task<AccessToken?> GetAccessTokenAsync(string name, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var token = await dbContext.Set<AccessTokenEntity>().FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        if (token is null)
        {
            return null;
        }

        return new AccessToken
        {
            ExpiresAt = token.ExpiresAt,
            IssuedAt = token.IssuedAt,
            Name = name,
            RefreshToken = token.RefreshToken,
            Token = token.AccessToken,
            TokenType = token.TokenType
        };
    }

    public async Task SaveAccessTokenAsync(AccessToken accessToken, CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var set = dbContext.Set<AccessTokenEntity>();
        var token = await set.FirstOrDefaultAsync(x => x.Name == accessToken.Name, cancellationToken);

        if (token is null)
        {
            set.Add(new AccessTokenEntity
            {
                AccessToken = accessToken.Token,
                ExpiresAt = accessToken.ExpiresAt,
                IssuedAt = accessToken.IssuedAt,
                Name = accessToken.Name,
                RefreshToken = accessToken.RefreshToken,
                TokenType = accessToken.TokenType
            });
        }
        else
        {
            token.AccessToken = accessToken.Token;
            token.ExpiresAt = accessToken.ExpiresAt;
            token.IssuedAt = accessToken.IssuedAt;
            token.RefreshToken = accessToken.RefreshToken;
            token.TokenType = accessToken.TokenType;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
