#define DISABLE_AZURE

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Serilog;

namespace AniNexus;

/// <summary>
/// A helper class to bootstrap the application.
/// </summary>
internal sealed partial class AniNexusApplicationBuilder
{
    private readonly AniNexusApplicationConfiguration _configuration;

    public AniNexusApplicationBuilder()
        : this(new())
    {
    }

    public AniNexusApplicationBuilder(AniNexusApplicationConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Configures the global Serilog logger.
    /// </summary>
    public AniNexusApplicationBuilder ConfigureGlobalLogger()
    {
        Log.Logger = _configuration.GetLoggerConfiguration().CreateLogger();

        return this;
    }

    /// <summary>
    /// Creates a pre-configured Serilog logger.
    /// </summary>
    public ILogger GetSerilogLogger<TContext>()
    {
        return Log.Logger.ForContext<TContext>();
    }

    /// <summary>
    /// Builds a <see cref="WebApplication"/> from this <see cref="AniNexusApplicationBuilder"/> instance.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <param name="environment">The name of the environment to run under.</param>
    public WebApplication? Build(string[] args, string? environment = null)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            EnvironmentName = environment
        });

        if (!_configuration.TryAddConfiguration(builder.Configuration, args, builder.Environment.EnvironmentName))
        {
            return null;
        }

        builder.Host
            .UseConsoleLifetime()
            .UseDefaultServiceProvider(static (ctx, o) => o.ValidateScopes = ctx.HostingEnvironment.IsDevelopment())
            .UseSerilog()
            .ConfigureServices(ConfigureServices);

#if !DISABLE_AZURE
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var oauthOptions = builder.Configuration.GetRequiredSection("OAuth20").Get<OAuth2Options>();

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = oauthOptions!.Issuer,
                    ValidAudience = oauthOptions.Audience,
                    IssuerSigningKey = oauthOptions.GetSecurityKey()
                };
            });

        builder.Services.AddFeatureManagement();
        builder.Services.AddAzureAppConfiguration();
        builder.Services.AddHostedService<AzureConfigurationRefresherService>();
#endif


        builder.Services.AddHttpClient();

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();

        // Remove unwanted services.
        builder.Services.RemoveAll<IHttpMessageHandlerBuilderFilter>();

        return builder.Build();
    }
}
