namespace lts.Constants;

public static class Constatns
{
    public static class partitionDTOs
    {
        public const int Min = 0;
        public const int Max = 10; 
        public const string ErrorMessage = "Partition must be a non-negative integer.";
    }

    public static class ControllerMessages
    {
        public const string StartListeningMessage = "Started listening on partition {0}";
        public const string StopListeningMessage = "Stopped listening on partition {0}";
    }

    public static class SwaggerConfig
    {
        public const string Title = "LTS API";
        public const string Version = "v1";
        public const string Description = "Kafka Consumer API for LTS";
        public const string Endpoint = "/swagger/v1/swagger.json";
        public const string RoutePrefix = "";
    }

    public static class Logging
    {
        public const string SwaggerUrl = "Swagger UI is running at: {0}";
        public const string ApplicationStarted = "LTS API started successfully";
        public const string ApplicationStarting = "LTS API is starting...";
        public const string EnvironmentDev = "Running in Development mode";
        public const string EnvironmentProd = "Running in Production mode";
        public const string KafkaConsumeError = "Kafka consume error: {0}";
    }

    public static class KafkaDefaults
    {
        public const string SectionName = "KafkaConsumer";
        public const string BootstrapServers = "localhost:9092";
        public const string Topic = "telemetry.packets.raw";
        public const string GroupIdPrefix = "raven-consumer";
        public const string GroupIdFormat = "{0}-{1}-{2}";
    }
}
