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

        // TelemetryStreamController messages
        public const string StreamStarted = "Telemetry streaming started for partition {0}";
        public const string StreamAlreadyRunning = "Stream for partition {0} is already running.";
        public const string StreamStopped = "Telemetry streaming stopped for partition {0}";
        public const string StreamNotFound = "No active stream found for partition {0}.";
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

    public static class TelemetryStreamMessages
    {
        public const string StreamAlreadyActive = "Stream for partition {Partition} is already active.";
        public const string StreamStartedSuccess = "Successfully started telemetry stream for partition {Partition}.";
        public const string StreamStopping = "Stopping telemetry stream for partition {Partition}...";
        public const string StreamNotRunning = "Cannot stop stream: partition {Partition} was not running.";
        public const string ConsumerWorkerAttached = "Kafka consumer worker attached and actively consuming partition {Partition}.";
        public const string PayloadReceived = "[Partition {Partition} | Offset {Offset}] Payload received: {Data}";
        public const string TelemetryFrameParsed = "[Partition {Partition} | Offset {Offset}] Parsed Telemetry Frame:\n{FrameSummary}";
        public const string ConsumerLoopCancelled = "Consume loop for partition {Partition} gracefully cancelled.";
        public const string ConsumerLoopError = "Unexpected error in consume loop for partition {Partition}.";
        public const string ConsumerResourcesCleanedUp = "Consumer resources cleaned up for partition {Partition}.";
        public const string ConsumeException = "Kafka consume exception: {Reason}";
    }
}
