using Api.Services;
using Microsoft.AspNetCore.Routing;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add configuration from environment variables and appsettings.json file
IConfigurationRoot config = builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services.AddCors();

WebApplication app = builder.Build();

string port = Environment.GetEnvironmentVariable("PORT") ?? "3001";

IEnumerable<string> cities = config.GetSection("AvailableCities").GetChildren().Where(c => c.Value != null).Select(c => c.Value!.ToLowerInvariant());

string? weatherApiKey = Environment.GetEnvironmentVariable("WeatherApiKey") ?? config["WeatherApiKey"];

if (string.IsNullOrEmpty(weatherApiKey))
{
    throw new Exception("WeatherApiKey is required to run the API");
}

string[] allowedOrigins = config.GetSection("AllowedOrigins").GetChildren().Where(c => c.Value != null).Select(c => c.Value!.ToLowerInvariant()).ToArray();

app.UseCors(builder =>
{
    builder.WithOrigins(allowedOrigins)
           .AllowAnyMethod()
           .AllowAnyHeader();
});


if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
{
    app.UseExceptionHandler(exceptionHandlerApp
    => exceptionHandlerApp.Run(async context
        => await Results.Problem()
                     .ExecuteAsync(context)));
}
else
{
    app.UseDeveloperExceptionPage();
}

await new Api.CitiesModule.CitiesModule()
    .WithAvailableCities(() => Task.FromResult(cities))
    .WithRoutes("cities", app)
    .WithLogging((level, message) => Console.WriteLine($"[{level}] {message}"))
    .InitializeAsync();

await new Api.WeatherModule.WeatherModule()
    .WithHttpClient(new WeatherApiHttpClient(weatherApiKey, "X-RapidAPI-Key", (level, message) => Console.WriteLine($"[{level}] {message}")))
    .WithAvailableCities(() => Task.FromResult(cities))
    .WithConfig(config)
    .WithRoutes("weather", app)
    .WithLogging((level, message) => Console.WriteLine($"[{level}] {message}"))
    .InitializeAsync();

app.MapGet("/", () => "Hello World!");
app.Run($"http://*:{port}");
