using AniNexus.MediaProviders.Anilist.Serializers;
using AniNexus.MediaProviders.MediaProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AniNexus.MediaProviders.Anilist;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Anilist and its dependencies to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration root.</param>
    public static IServiceCollection UseAnilistMediaProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AnilistOptions>(configuration.GetSection("Anilist"));
        services.AddAnilistClient().ConfigureHttpClient(static (s, c) =>
        {
            var config = s.GetRequiredService<AnilistTokenProvider>();
            config.ConfigureHttpClient(c);
        });

        services.AddSerializer<JsonScalarSerializer>();
        services.AddSingleton<IMediaProvider, AnilistMediaProvider>();
        services.AddSingleton<AnilistTokenProvider>();
        services.AddHostedService(static p => p.GetRequiredService<AnilistTokenProvider>());

        return services;
    }
}
