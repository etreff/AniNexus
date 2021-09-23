using AniNexus.Repository.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AniNexus.Repository;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAniNexusProviders(this IServiceCollection services)
    {
        services.TryAddScoped<IRepositoryProvider, DefaultRepositoryProvider>();

        return services;
    }
}
