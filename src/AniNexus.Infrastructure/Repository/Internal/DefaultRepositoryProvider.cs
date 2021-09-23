using AniNexus.Domain;
using AniNexus.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AniNexus.Repository.Internal;

internal class DefaultRepositoryProvider : IRepositoryProvider
{
    private readonly ApplicationDbContext DbContext;
    private readonly IOptions<JwtSettings> JwtSettings;
    private readonly ILoggerFactory LoggerFactory;

    public DefaultRepositoryProvider(
        IDbContextFactory<ApplicationDbContext> dbContextFactory,
        IOptions<JwtSettings> jwtSettings,
        ILoggerFactory loggerFactory)
    {
        DbContext = dbContextFactory.CreateDbContext();
        JwtSettings = jwtSettings;
        LoggerFactory = loggerFactory;
    }

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await DbContext.SaveChangesAsync();
        await DbContext.DisposeAsync();
    }

    public IAnimeRepository GetAnimeRepository()
    {
        return new DefaultAnimeRepository(DbContext, LoggerFactory.CreateLogger<DefaultAnimeRepository>());
    }

    public IUserRepository GetUserRepository()
    {
        return new DefaultUserRepository(DbContext, JwtSettings, LoggerFactory.CreateLogger<DefaultUserRepository>());
    }
}
