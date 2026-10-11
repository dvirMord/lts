using Confluent.Kafka;
using Microsoft.Extensions.Options;

using lts.Configuration;
using lts.Interfaces;
using static lts.Constants.Constatns.KafkaDefaults;
using static lts.Constants.Constatns.Logging;

namespace lts.Services;
public class KafkaConsumerService : IKafkaConsumerService
{
    private readonly IConsumer<Null, string> _kafkaConsumer;

    public KafkaConsumerService(IOptions<KafkaConsumerConfig> configOptions, int partition)
    {
        KafkaConsumerConfig settings = configOptions.Value;

        ConsumerConfig consumerConfig = new ConsumerConfig
        {
            BootstrapServers = settings.BootstrapServers,
            GroupId = string.Format(GroupIdFormat, settings.GroupIdPrefix, settings.Topic, partition),
            EnableAutoCommit = settings.EnableAutoCommit,
            AutoOffsetReset = settings.AutoOffsetReset
        };

        this._kafkaConsumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();

        TopicPartition topicPartition = new TopicPartition(settings.Topic, new Partition(partition));
        this._kafkaConsumer.Assign(topicPartition);
    }

    public ConsumeResult<Null, string>? ConsumeTelemetryFrames(CancellationToken ct = default)
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
            Console.WriteLine(string.Format(KafkaConsumeError, ex.Error.Reason));
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