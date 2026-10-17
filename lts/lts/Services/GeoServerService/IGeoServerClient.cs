using lts.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace lts.Interfaces;

public interface IGeoServerClient
{
    Task StartListeningAsync(CancellationToken ct = default);
    Task<bool> SendTelemetryFrameAsync(RaycastRequestDto requestPayload, CancellationToken ct = default);
    bool IsClientConnected { get; }
}