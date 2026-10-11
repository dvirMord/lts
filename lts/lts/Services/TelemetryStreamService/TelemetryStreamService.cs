using Confluent.Kafka;
using lts.DTOs;
using lts.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static lts.Constants.Constatns;

namespace lts.Services;
public class TelemetryStreamService: ITelemetryStreamService
{
    private readonly IKafkaConsumerFactory _kafkaConsumerFactory;
    private readonly ILogger<TelemetryStreamService> _logger;
    private readonly ConcurrentDictionary<int, CancellationTokenSource> _activeStreams = new();
    public TelemetryStreamService(IKafkaConsumerFactory kafkaConsumerFactory, ILogger<TelemetryStreamService> logger)
    {
        this._kafkaConsumerFactory = kafkaConsumerFactory;
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
        Task.Run(() => ConsumeLoop(partition, cts.Token), cts.Token);
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

    private void ConsumeLoop(int partition, CancellationToken ct)
    {
        using var consumer = this._kafkaConsumerFactory.CreateConsumer(partition);

        this._logger.LogInformation(TelemetryStreamMessages.ConsumerWorkerAttached, partition);

        try
        {
            while (!ct.IsCancellationRequested)
            {
                var result = consumer.ConsumeTelemetryFrames(ct);

                if (result?.Message?.Value != null)
                {
                    string rawJson = result.Message.Value;

                    try
                    {
                        var frame = JsonSerializer.Deserialize<KafkaTelemetryFrame>(rawJson);
                        if (frame != null)
                        {
                            string frameSummary = frame.ToFormattedSummary(result.Partition.Value, result.Offset.Value);
                            this._logger.LogInformation(TelemetryStreamMessages.TelemetryFrameParsed,
                                result.Partition.Value,
                                result.Offset.Value,
                                frameSummary);
                        }
                        else
                        {
                            this._logger.LogInformation(TelemetryStreamMessages.PayloadReceived,
                                result.Partition.Value,
                                result.Offset.Value,
                                rawJson);
                        }
                    }
                    catch (JsonException)
                    {
                        this._logger.LogInformation(TelemetryStreamMessages.PayloadReceived,
                            result.Partition.Value,
                            result.Offset.Value,
                            rawJson);
                    }
                }
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
}