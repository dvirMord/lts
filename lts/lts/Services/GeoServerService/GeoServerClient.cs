using lts.Interfaces;
using lts.DTOs;
using Microsoft.Extensions.Logging;
using static lts.Constants.Constatns.GeoServerMessages;

namespace lts.Services.GeoServerService;
public class GeoServerClient : IGeoServerClient
{
    private readonly ILogger<GeoServerClient> _logger;
    private readonly HttpClient _httpClient;

    public GeoServerClient(HttpClient httpClient, ILogger<GeoServerClient> logger)
    {
        this._httpClient = httpClient;
        this._logger = logger;
    }

    public async Task<RaycastResponseDto?> CalculateIntersectionAsync(RaycastRequestDto request, CancellationToken ct = default)
    {
        try
        {
            var response = await this._httpClient.PostAsJsonAsync(lts.Constants.Constatns.GeoServerConfig.RaycastEndpoint, request, ct);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<RaycastResponseDto>(cancellationToken: ct);
            }

            this._logger.LogWarning(UnexpectedStatusCode, response.StatusCode);
            return null;
        }
        catch (OperationCanceledException)
        {
            return null;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, CommunicationError);
            return null;
        }
    }
}
