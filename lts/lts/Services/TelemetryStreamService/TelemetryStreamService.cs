using Confluent.Kafka;
using lts.DTOs;
using lts.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static lts.Constants.Constants;
using static lts.Constants.Constants.GeoServerSocketMessages;

namespace lts.Services;
public class TelemetryStreamService: ITelemetryStreamService
{
    private readonly IKafkaConsumerFactory _kafkaConsumerFactory;
    private readonly IGeoServerClient _geoServerClient;
    private readonly ILogger<TelemetryStreamService> _logger;
    private readonly ConcurrentDictionary<int, CancellationTokenSource> _activeStreams = new();
    public TelemetryStreamService(IKafkaConsumerFactory kafkaConsumerFactory, IGeoServerClient geoServerClient, ILogger<TelemetryStreamService> logger)
    {
        this._kafkaConsumerFactory = kafkaConsumerFactory;
        this._geoServerClient = geoServerClient;
        this._logger = logger;
    }

    public bool StartStream(int partition)
    {
        if (_activeStreams.ContainsKey(partition))
        {
            this._logger.LogWarning(TelemetryStreamMessages.StreamAlreadyActive, partition);
            return false;
        }
        var cts = new CancellationTokenSource();
        if (!_activeStreams.TryAdd(partition, cts))
        {
            return false;
        }
        Task.Run(async () => await ConsumeLoop(partition, cts.Token), cts.Token);
        this._logger.LogInformation(TelemetryStreamMessages.StreamStartedSuccess, partition);
        return true;
    }

    public bool StopStream(int partition)
    {
        if (this._activeStreams.TryRemove(partition, out var cts))
        {
            this._logger.LogInformation(TelemetryStreamMessages.StreamStopping, partition);
            cts.Cancel();
            cts.Dispose();
            return true;
        }

        this._logger.LogWarning(TelemetryStreamMessages.StreamNotRunning, partition);
        return false;
    }

    public bool IsStreamActive(int partition)
    {
        return this._activeStreams.ContainsKey(partition);
    }

    public IReadOnlyList<int> GetActiveStreams()
    {
        return this._activeStreams.Keys.ToList();
    }

    private async Task ConsumeLoop(int partition, CancellationToken ct)
    {
        using var consumer = this._kafkaConsumerFactory.CreateConsumer(partition);
        this._logger.LogInformation(TelemetryStreamMessages.ConsumerWorkerAttached, partition);

        try
        {
            while (!ct.IsCancellationRequested)
            {
                var result = consumer.ConsumeTelemetryFrames(ct);
                if (result?.Message?.Value is null)
                {
                    continue;
                }

                await ProcessConsumeResultAsync(result, ct);
            }
        }
        catch (ConsumeException ex)
        {
            this._logger.LogError(ex, TelemetryStreamMessages.ConsumeException, ex.Error.Reason);
        }
        catch (OperationCanceledException)
        {
            this._logger.LogInformation(TelemetryStreamMessages.ConsumerLoopCancelled, partition);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, TelemetryStreamMessages.ConsumerLoopError, partition);
        }
        finally
        {
            consumer.Close();
            this._activeStreams.TryRemove(partition, out _);
            this._logger.LogInformation(TelemetryStreamMessages.ConsumerResourcesCleanedUp, partition);
        }
    }

    private async Task ProcessConsumeResultAsync(ConsumeResult<string?, string> result, CancellationToken ct)
    {
        string rawJson = result.Message.Value;
        var frame = TryDeserializeFrame(rawJson);

        if (frame is null)
        {
            this._logger.LogInformation(
                TelemetryStreamMessages.PayloadReceived,
                result.Partition.Value,
                result.Offset.Value,
                rawJson);
            return;
        }

        LogParsedFrame(result, frame);
        await DispatchFrameAsync(result.Partition.Value, frame, ct);
    }

    private KafkaTelemetryFrame? TryDeserializeFrame(string rawJson)
    {
        try
        {
            return JsonSerializer.Deserialize<KafkaTelemetryFrame>(rawJson);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private void LogParsedFrame(ConsumeResult<string?, string> result, KafkaTelemetryFrame frame)
    {
        string frameSummary = frame.ToFormattedSummary(result.Partition.Value, result.Offset.Value);
        this._logger.LogInformation(
            TelemetryStreamMessages.TelemetryFrameParsed,
            result.Partition.Value,
            result.Offset.Value,
            frameSummary);
    }

    private async Task DispatchFrameAsync(int partition, KafkaTelemetryFrame frame, CancellationToken ct)
    {
        RaycastRequestDto raycastRequest = frame.ToRaycastRequest();
        bool sent = await this._geoServerClient.SendTelemetryFrameAsync(raycastRequest, ct);

        if (!sent)
        {
            this._logger.LogWarning(ClientNotActive, partition);
            return;
        }

        this._logger.LogInformation(FrameSentSuccess, partition);
    }
}