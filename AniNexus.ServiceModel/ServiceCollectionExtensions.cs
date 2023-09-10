using AniNexus.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AniNexus.ServiceModel;

/// <summary>
/// <see cref="IServiceCollection"/> extensions.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the authentication services to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the authentication services to.</param>
    public static IServiceCollection AddAniNexusAuthentication(this IServiceCollection services)
    {
        services.AddOptions<OtpOptions>("Auth:Otp");
        services.AddOptions<PasswordHashingOptions>("Auth:Pass");
        services.AddOptions<OAuth2Options>("Auth:OAuth2");

        services.AddSingleton<IOtpProvider, OtpProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
