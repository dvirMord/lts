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
    }
}
