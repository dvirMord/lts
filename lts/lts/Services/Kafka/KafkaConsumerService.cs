using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

using lts.Configuration;
using lts.Interfaces;
using static lts.Constants.Constatns.KafkaDefaults;
using static lts.Constants.Constatns.Logging;

namespace lts.Services;
public class KafkaConsumerService : IKafkaConsumerService
{
    private readonly IConsumer<string?, string> _kafkaConsumer;
    private readonly ILogger<KafkaConsumerService> _logger;

    public KafkaConsumerService(IOptions<KafkaConsumerConfig> configOptions, int partition, ILogger<KafkaConsumerService> logger)
    {
        this._logger = logger;
        KafkaConsumerConfig settings = configOptions.Value;

        ConsumerConfig consumerConfig = new ConsumerConfig
        {
            BootstrapServers = settings.BootstrapServers,
            GroupId = string.Format(GroupIdFormat, settings.GroupIdPrefix, settings.Topic, partition),
            EnableAutoCommit = settings.EnableAutoCommit,
            AutoOffsetReset = settings.AutoOffsetReset
        };

        this._kafkaConsumer = new ConsumerBuilder<string?, string>(consumerConfig).Build();

        TopicPartition topicPartition = new TopicPartition(settings.Topic, new Partition(partition));
        this._kafkaConsumer.Assign(topicPartition);
    }

    public ConsumeResult<string?, string>? ConsumeTelemetryFrames(CancellationToken ct = default)
    {
        try
        {
            return _kafkaConsumer.Consume(ct);
        }
        catch (OperationCanceledException)
        {
            return null;
        }
        catch (ConsumeException ex)
        {
            this._logger.LogError(ex, KafkaConsumeError, ex.Error.Reason);
            return null;
        }
    }

    public void Close()
    {
        _kafkaConsumer.Close();
    }

    public void Dispose()
    {
        _kafkaConsumer?.Dispose();
    }
}