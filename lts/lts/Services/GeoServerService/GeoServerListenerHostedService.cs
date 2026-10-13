using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using lts.Interfaces;
using static lts.Constants.Constants.GeoServerSocketMessages;

namespace lts.Services;

public class GeoServerListenerHostedService : BackgroundService
{
    private readonly IGeoServerClient _geoServerClient;
    private readonly ILogger<GeoServerListenerHostedService> _logger;

    public GeoServerListenerHostedService(
        IGeoServerClient geoServerClient,
        ILogger<GeoServerListenerHostedService> logger)
    {
        _geoServerClient = geoServerClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(ListenerStarting);
        await _geoServerClient.StartListeningAsync(stoppingToken);
    }
}