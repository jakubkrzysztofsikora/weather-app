namespace Api;

public interface IModule
{
    IModule WithConfig(IConfigurationRoot config);
    IModule WithRoutes(string rootPath, WebApplication app);
    IModule WithLogging(Action<LogLevel, string> log);
    Task<IModule> InitializeAsync();
}