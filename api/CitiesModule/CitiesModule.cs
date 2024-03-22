
using Api.CitiesModule.Models;
using Api.WeatherModule.Services;

namespace Api.CitiesModule;

public class CitiesModule : IModule
{
    private Func<Task<IEnumerable<string>>>? _getAvailableCitiesAsync;
    private Action<LogLevel, string>? _log;
    private WebApplication? _app;
    private string _rootPath = "cities";
    
    public async Task<IModule> InitializeAsync()
    {
        if (_getAvailableCitiesAsync == null)
        {
            throw new Exception("Available cities fetching function is required");
        }

        if (_app == null)
        {
            throw new Exception("App is required to bind routes");
        }

        IEnumerable<string> cities = (await _getAvailableCitiesAsync()).Select(c => c.ToLowerInvariant());

        _app.MapGet($"/{_rootPath}", async (context) =>
        {
            await new NullObjectResponse<IEnumerable<City>>(cities.Select(cityName => new City { Name = cityName }), context).ToResponse();
        });

        return this;
    }

    public IModule WithConfig(IConfigurationRoot config)
    {
        return this;
    }

    public IModule WithLogging(Action<LogLevel, string> log)
    {
        _log = log;
        return this;
    }

    public IModule WithRoutes(string rootPath, WebApplication app)
    {
        _rootPath = rootPath;
        _app = app;
        return this;
    }

    public CitiesModule WithAvailableCities(Func<Task<IEnumerable<string>>> getAvailableCitiesAsync)
    {
        _getAvailableCitiesAsync = getAvailableCitiesAsync;
        return this;
    }
}