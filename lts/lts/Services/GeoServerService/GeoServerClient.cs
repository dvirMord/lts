using Confluent.Kafka;
using lts.DTOs;
using lts.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using static lts.Constants.Constants;
using static lts.Constants.Constants.GeoServerSocketMessages;

namespace lts.Services.GeoServerService;
public class GeoServerClient : IGeoServerClient
{
    private readonly ILogger<GeoServerClient> _logger;
    private TcpListener? _listener;
    private TcpClient? _activeClient;
    private NetworkStream? _networkStream;
    private readonly SemaphoreSlim _sendLock = new(1, 1);
    public bool IsClientConnected => _activeClient != null && _activeClient.Connected;
    public GeoServerClient(ILogger<GeoServerClient> logger)
    {
        this._logger = logger;
    }

    public async Task StartListeningAsync(CancellationToken ct = default)
    {
        try
        {
            this._listener = new TcpListener(IPAddress.Parse(GeoServerConfig.ip), GeoServerConfig.port);
            this._listener.Start();
            this._logger.LogInformation(ListeningStarted, GeoServerConfig.ip, GeoServerConfig.port);

            while (!ct.IsCancellationRequested)
            {
                TcpClient geoServerClient = await this._listener.AcceptTcpClientAsync(ct);
                this._logger.LogInformation(ClientConnected, geoServerClient.Client.RemoteEndPoint);

                this._activeClient?.Dispose();
                this._networkStream?.Dispose();

                this._activeClient = geoServerClient;
                this._networkStream = geoServerClient.GetStream();
            }
        }
        catch (Exception ex)
        {
            if (ex is OperationCanceledException)
            {
                this._logger.LogInformation(ListenerStopped);
            }
            else
            {
                this._logger.LogError(ex, ListenerError);
            }
        }
        finally
        {
            this._listener?.Stop(); 
        }
    }

    public async Task<bool> SendTelemetryFrameAsync(RaycastRequestDto requestPayload, CancellationToken ct = default)
    {
        if (_networkStream == null || !IsClientConnected)
        {
            return false;
        }

        string jsonString = JsonSerializer.Serialize(requestPayload);
        byte[] data = Encoding.UTF8.GetBytes(jsonString + NewlineCharacter);

        await this._sendLock.WaitAsync(ct);
        try
        {
            if (_networkStream == null || !IsClientConnected)
            {
                return false;
            }

            await this._networkStream.WriteAsync(data, ct);
            await this._networkStream.FlushAsync(ct);
            return true;
        }
        catch (Exception ex)
        {
            this._logger.LogWarning(StreamFrameError, ex.Message);
            CleanupConnection();
            return false;
        }
        finally
        {
            this._sendLock.Release();
        }
    }

    private void CleanupConnection()
    {
        this._networkStream?.Dispose();
        this._networkStream = null;
        this._activeClient?.Dispose();
        this._activeClient = null;
    }
}
