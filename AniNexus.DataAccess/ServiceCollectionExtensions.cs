using AniNexus.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AniNexus.DataAccess;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the AniNexus database context to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    public static IServiceCollection AddAniNexusDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AniNexusDbOptions>(configuration.GetSection("AniNexus"));
        services.AddEntityFrameworkSqlServer();
        services.AddPooledDbContextFactory<AniNexusDbContext>((p, options) =>
        {
            var config = p.GetRequiredService<IOptions<AniNexusDbOptions>>().Value;
            options.UseSqlServer(config.DbConnection);
            options.UseInternalServiceProvider(p);
        });

        services.TryAddSingleton<IAuthTokenStore, AuthTokenStore>();

        return services;
    }
}
