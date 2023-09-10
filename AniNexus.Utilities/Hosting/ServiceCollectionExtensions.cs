using AniNexus.Reflection;
using CommunityToolkit.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace AniNexus.Hosting;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds an <see cref="IHostedService"/> registration for the given type.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="type">The service type.</param>
    public static IServiceCollection AddHostedService(this IServiceCollection services, Type type)
    {
        if (!type.IsTypeOf<IHostedService>())
        {
            ThrowHelper.ThrowArgumentException(nameof(type), "Type must implement IHostedService.");
        }

        services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), type));

        return services;
    }

    /// <summary>
    /// Adds an <see cref="IHostedService"/> registration for the given type and
    /// registers the type as a singleton in the DI container.
    /// </summary>
    /// <typeparam name="THostedService">The service type.</typeparam>
    /// <param name="services">The service collection.</param>
    public static IServiceCollection AddExposedHostedService<THostedService>(this IServiceCollection services)
        where THostedService : class, IHostedService
    {
        return services
            .AddSingleton<THostedService>()
            .AddHostedService(s => s.GetRequiredService<THostedService>());
    }
}
