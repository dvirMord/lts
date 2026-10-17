namespace lts.Constants;

public static class Constants
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
        public const string RaycastIntersectionResult = "[Partition {Partition}] Raycast intersection result: HasIntersection={HasIntersection}, Lat={Lat}, Lon={Lon}, Alt={Alt}m, SlantRange={SlantRange}m";
    }

    public static class GeoServerConfig
    {
        public const string BaseAddress = "http://localhost:18080";
        public const string RaycastEndpoint = "/api/v1/calculate";
        public const int TimeoutSeconds = 2;
        public const int port = 18080;
        public const string ip = "127.0.0.1";
    }

    public static class GeoServerMessages
    {
        public const string UnexpectedStatusCode = "GeoServer returned unexpected status code: {StatusCode}";
        public const string CommunicationError = "Failed to communicate with GeoServer raycast endpoint.";
    }

    public static class GeoServerSocketMessages
    {
        public const string ListenerStarting = "Starting GeoServer background socket listener...";
        public const string ListeningStarted = "GeoServer Socket listening on {Ip}:{Port}...";
        public const string ClientConnected = "New client connected to LTS Socket: {RemoteEndpoint}";
        public const string ListenerStopped = "GeoServer listener gracefully stopped.";
        public const string ListenerError = "Error occurred in GeoServer socket listener.";
        public const string FrameSentSuccess = "[Partition {Partition}] Telemetry frame successfully streamed to GeoServer socket client.";
        public const string ClientNotActive = "[Partition {Partition}] No active socket client connected or send failed.";
        public const string StreamFrameError = "Failed to stream frame to client: {Message}";
        public const string NewlineCharacter = "\n";
    }

    public static class TelemetryStreamAdditionalMessages
    {
        public const string SwaggerUrlLocalhost = "http://localhost:5094";
    }
}
