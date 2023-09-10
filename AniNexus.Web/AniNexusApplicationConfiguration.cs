#define DISABLE_AZURE

using System;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace AniNexus;

/// <summary>
/// Configuration used to bootstrap the application host.
/// </summary>
internal sealed class AniNexusApplicationConfiguration
{
    /// <summary>
    /// Creates a pre-configured Serilog logger configuration.
    /// </summary>
    public LoggerConfiguration GetLoggerConfiguration()
    {
        var configuration = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .Enrich.FromLogContext();

        string? seq = Environment.GetEnvironmentVariable("ANINEXUS_SEQ");
        return !string.IsNullOrWhiteSpace(seq)
            ? configuration.WriteTo.Seq(seq)
            : configuration.WriteTo.Console();
    }

    /// <summary>
    /// Tries to add the AniNexus configuration to the provided <see cref="IConfigurationBuilder"/>.
    /// </summary>
    /// <param name="configurationBuilder">The constructed configuration.</param>
    /// <param name="args">The command line arguments.</param>
    /// <param name="environment">The name of the environment to run under.</param>
    public bool TryAddConfiguration(IConfigurationBuilder configurationBuilder, string[] args, string environment)
    {
        var builder = new ConfigurationBuilder()
            .AddEnvironmentVariables("DOTNET_")
            .AddEnvironmentVariables("ANINEXUS_")
            .AddUserSecrets(typeof(AniNexusApplicationConfiguration).Assembly)
            .AddCommandLine(args)
            .AddJsonFile("appsettings.json", true, false)
            .AddJsonFile($"appsettings.{environment}.json", true, false);

#if !DISABLE_AZURE
        // We need a two pass build so we can get our Azure credentials.
        var tempConfiguration = builder.Build();
        var azureCredentials = tempConfiguration.GetRequiredSection("Azure").Get<AzureCredentials>();

        configurationBuilder.AddConfiguration(tempConfiguration);

        return ConfigureAzure(builder, azureCredentials, environment);
#else
        configurationBuilder.AddConfiguration(builder.Build());
        return true;
#endif
    }

#if !DISABLE_AZURE
    private static bool ConfigureAzure(IConfigurationBuilder configurationBuilder, AzureCredentials? credentials, string environment)
    {
        if (credentials is null || !credentials.IsValid())
        {
            return false;
        }

        if (!ImportAzureCertificate(credentials.CertPass))
        {
            return false;
        }

        if (!TryGetAzureClientCertificateCredentials(credentials, out var clientCertificateCredentials))
        {
            return false;
        }

        configurationBuilder.AddAzureAppConfiguration(options => options
            .ConfigureClientOptions(static o =>
            {
                o.Diagnostics.IsLoggingEnabled = true;
                o.Diagnostics.IsLoggingContentEnabled = false;
                o.Diagnostics.IsDistributedTracingEnabled = false;

                o.Retry.Mode = Azure.Core.RetryMode.Exponential;
                o.Retry.MaxRetries = int.MaxValue;
                o.Retry.MaxDelay = TimeSpan.FromMinutes(30);
                o.Retry.NetworkTimeout = TimeSpan.FromMinutes(5);
            })
            .Connect(credentials.ConnectionString!)
            .ConfigureKeyVault(kv => kv.SetCredential(clientCertificateCredentials))
            // Load all keys with no filter.
            .Select(KeyFilter.Any, LabelFilter.Null)
            // Load all keys for the current environment
            .Select(KeyFilter.Any, environment)
            // Configure the sentinel and cache expiration
            .ConfigureRefresh(static r => r
                .Register("Sentinel", true)
                .SetCacheExpiration(TimeSpan.FromMinutes(20)))
            .UseFeatureFlags()
        );

        return true;
    }

    private static bool ImportAzureCertificate(string certPass)
    {
        const string certFile = "certs/AniNexus.pfx";
        if (File.Exists(certFile))
        {
            try
            {
                using var cert = new X509Certificate2(certFile, certPass, X509KeyStorageFlags.PersistKeySet);
                using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser, OpenFlags.ReadWrite);
                store.Add(cert);

                store.Close();

                try
                {
                    File.Delete(certFile);
                }
                catch
                {
                    // Suppress
                }

                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Fatal("Unable to import PFX - " + e.Message);
                return false;
            }
        }

#if DEBUG
        // If we are running locally, this may be saved on the developer's machine and the file may not exist.
        // This is not an error condition, and it will be verified in the next check.
        return true;
#else
        return false;
#endif
    }

    private static bool TryGetAzureClientCertificateCredentials(AzureCredentials credentials, [NotNullWhen(true)] out ClientCertificateCredential? clientCertificateCredentials)
    {
        clientCertificateCredentials = null;

        using var x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

        x509Store.Open(OpenFlags.ReadOnly);
        var cert = x509Store.Certificates
            .Find(X509FindType.FindByThumbprint, credentials.CertThumbprint!, false)
            .SingleOrDefault(f => f != null);

        if (cert is null)
        {
            Log.Logger.Fatal("Unable to load PFX - The certificate was not found under the current user's X509Store.");
            return false;
        }

        clientCertificateCredentials = new(credentials.TenantId, credentials.ClientId, cert);
        return true;
    }

    private class AzureCredentials
    {
        /// <summary>
        /// The .pfx password.
        /// </summary>
        public string? CertPass { get; set; }

        /// <summary>
        /// The certificate thumbprint.
        /// </summary>
        public string? CertThumbprint { get; set; }

        /// <summary>
        /// The Azure subscription TenantId.
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// The Azure application ClientId.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// The Azure configuration store connection string.
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Returns whether all required credential properties are set.
        /// </summary>
        [MemberNotNullWhen(true, nameof(CertPass), nameof(CertThumbprint), nameof(TenantId), nameof(ClientId), nameof(ConnectionString))]
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(CertPass) &&
                   !string.IsNullOrWhiteSpace(CertThumbprint) &&
                   !string.IsNullOrWhiteSpace(TenantId) &&
                   !string.IsNullOrWhiteSpace(ClientId) &&
                   !string.IsNullOrWhiteSpace(ConnectionString);
        }
    }
#endif
}
