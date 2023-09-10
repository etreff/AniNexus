using System;
using System.Threading;
using System.Threading.Tasks;
using AniNexus.Threading;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AniNexus.Services.Background;

internal sealed class AzureConfigurationRefresherService : BackgroundService
{
    private readonly IConfigurationRefresherProvider _refresherProvider;
    private readonly ILogger _logger;

    public AzureConfigurationRefresherService(IConfigurationRefresherProvider configurationRefresherProvider, ILogger<AzureConfigurationRefresherService> logger)
    {
        _refresherProvider = configurationRefresherProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        while (await stoppingToken.IsNotCancelledBeforeAsync(TimeSpan.FromMinutes(5)))
        {
            _logger.LogInformation("Refreshing configuration.");

            foreach (var refresher in _refresherProvider.Refreshers)
            {
                if (!await refresher.TryRefreshAsync(stoppingToken))
                {
                    _logger.LogWarning("Unable to refresh configuration to endpoint {Endpoint}.", refresher.AppConfigurationEndpoint);
                }
            }

            _logger.LogInformation("Configuration refresh complete.");
        }
    }
}
