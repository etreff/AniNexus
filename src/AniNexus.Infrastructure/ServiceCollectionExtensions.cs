using AniNexus.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AniNexus.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAniNexusProviders(this IServiceCollection services)
        {
            services.TryAddScoped<IRepositoryProvider, DefaultRepositoryProvider>();
            services.TryAddScoped<IAnimeRepository, DefaultAnimeRepository>();

            return services;
        }
    }
}
