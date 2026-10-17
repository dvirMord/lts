using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using lts.Configuration;
using lts.Interfaces;

namespace lts.Services;
public class KafkaConsumerFactory : IKafkaConsumerFactory
{
    private readonly IOptions<KafkaConsumerConfig> _options;
    private readonly ILoggerFactory _loggerFactory;

    public KafkaConsumerFactory(IOptions<KafkaConsumerConfig> options, ILoggerFactory loggerFactory)
    {
        this._options = options;
        this._loggerFactory = loggerFactory;
    }

    public IKafkaConsumerService CreateConsumer(int partition)
    { 
        var logger = _loggerFactory.CreateLogger<KafkaConsumerService>();
        return new KafkaConsumerService(_options, partition, logger);
    }
}