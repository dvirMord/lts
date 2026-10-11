using Confluent.Kafka;
using static lts.Constants.Constatns.KafkaDefaults;

namespace lts.Configuration;
public class KafkaConsumerConfig
{
    public const string SectionName = "KafkaConsumer";

    public string BootstrapServers { get; set; } = "localhost:9092";
    public string Topic { get; set; } = "telemetry.packets.raw";
    public string GroupIdPrefix { get; set; } = "raven-consumer";
    public bool EnableAutoCommit { get; set; } = true;
    public  AutoOffsetReset AutoOffsetReset { get; set; } = AutoOffsetReset.Latest;
}
