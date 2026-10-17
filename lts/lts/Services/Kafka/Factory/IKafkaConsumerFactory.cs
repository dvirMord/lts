namespace lts.Interfaces;
public interface IKafkaConsumerFactory
{
    IKafkaConsumerService CreateConsumer(int partition);
}