using Confluent.Kafka;

namespace lts.Interfaces;

public interface IKafkaConsumerService : IDisposable
{
    ConsumeResult<Null, string>? ConsumeTelemetryFrames(CancellationToken ct = default);
    void Close();
}