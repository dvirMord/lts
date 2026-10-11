using Confluent.Kafka;

namespace lts.Interfaces;

public interface IKafkaConsumerService : IDisposable
{
    ConsumeResult<string?, string>? ConsumeTelemetryFrames(CancellationToken ct = default);
    void Close();
}