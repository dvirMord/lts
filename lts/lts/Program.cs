using lts.Configuration;
using lts.Interfaces;
using lts.Services;
using lts.Services.GeoServerService;
using static lts.Constants.Constatns;
using static lts.Constants.Constatns.SwaggerConfig;
using static lts.Constants.Constatns.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(SwaggerConfig.Version, new()
    {
        Title = SwaggerConfig.Title,
        Version = SwaggerConfig.Version,
        Description = SwaggerConfig.Description
    });
});

builder.Services.AddSingleton<IKafkaConsumerFactory, KafkaConsumerFactory>();
builder.Services.AddSingleton<ITelemetryStreamService, TelemetryStreamService>();
builder.Services.AddHttpClient<IGeoServerClient, GeoServerClient>(client =>
{
    client.BaseAddress = new Uri(GeoServerConfig.BaseAddress);
    client.Timeout = TimeSpan.FromSeconds(GeoServerConfig.TimeoutSeconds);
});

// Kafka Configuration
builder.Services.Configure<KafkaConsumerConfig>(
    builder.Configuration.GetSection(KafkaConsumerConfig.SectionName));

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation(ApplicationStarting);
logger.LogInformation(app.Environment.IsDevelopment() ? EnvironmentDev : EnvironmentProd);

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(SwaggerConfig.Endpoint, $"{SwaggerConfig.Title} {SwaggerConfig.Version}");
        options.RoutePrefix = SwaggerConfig.RoutePrefix;
    });

    logger.LogInformation(SwaggerUrl, "http://localhost:5000");
}

app.MapControllers();

logger.LogInformation(ApplicationStarted);

app.Run();