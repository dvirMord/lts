using static lts.Constants.Constatns.SwaggerConfig;
using static lts.Constants.Constatns.Logging;
using SwaggerConfig = lts.Constants.Constatns.SwaggerConfig;

var builder = WebApplication.CreateBuilder(args);

// Remove HTTPS from Kestrel to use HTTP only
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenLocalhost(5000); // HTTP on port 5000
});

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container
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

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation(ApplicationStarting);
logger.LogInformation(app.Environment.IsDevelopment() ? EnvironmentDev : EnvironmentProd);

// Configure the HTTP request pipeline
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

app.UseAuthorization();
app.MapControllers();

logger.LogInformation(ApplicationStarted);
app.Run();

