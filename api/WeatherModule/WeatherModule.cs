
using Api.WeatherModule.Controllers;
using Api.WeatherModule.Models;
using Api.WeatherModule.Ports;
using Api.WeatherModule.Services;

namespace Api.WeatherModule;

public class WeatherModule : IModule
{
    private IConfigurationRoot? _config;
    private WebApplication? _app;
    private IWeatherApiHttpClient? _httpClient;
    private string _rootPath = "weather";
    private Func<Task<IEnumerable<string>>>? _getAvailableCitiesAsync;
    private Action<LogLevel, string>? _log;

    public async Task<IModule> InitializeAsync()
    {
        if (_config == null)
        {
            throw new Exception("Config is required");
        }

        if (_app == null)
        {
            throw new Exception("App is required to bind routes");
        }

        if (_httpClient == null)
        {
            throw new Exception("HttpClient is required");
        }

        if (_getAvailableCitiesAsync == null)
        {
            throw new Exception("Available cities are required");
        }

        IEnumerable<string> availableCities = (await _getAvailableCitiesAsync()).Select(c => c.ToLowerInvariant());

        _app.MapGet($"/{_rootPath}/{{city}}", async (context) =>
        {
            WeatherOutput? result = await new WeatherController(_httpClient, availableCities, _log)
                .GetWeatherAsync(context.GetRouteValue("city")?.ToString() ?? string.Empty);
            await new NullObjectResponse<WeatherOutput>(result, context).ToResponse();
        });
        _app.MapGet($"/{_rootPath}/health", () => new { Message = "Healthy" });

        return this;
    }

    public IModule WithConfig(IConfigurationRoot config)
    {
        _config = config;

        return this;
    }

    public IModule WithRoutes(string rootPath, WebApplication app)
    {
        _rootPath = rootPath;
        _app = app;

        return this;
    }

    public IModule WithLogging(Action<LogLevel, string> log)
    {
        _log = (logLevel, text) => log?.Invoke(logLevel, $"[{nameof(WeatherModule)}]: {text}");

        return this;
    }

    public WeatherModule WithAvailableCities(Func<Task<IEnumerable<string>>> availableCities)
    {
        _getAvailableCitiesAsync = availableCities;

        return this;
    }

    public WeatherModule WithHttpClient(IWeatherApiHttpClient httpClient)
    {
        _httpClient = httpClient;

        return this;
    }
}