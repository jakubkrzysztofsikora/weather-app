using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Api.WeatherModule;
using Api.WeatherModule.Ports;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ApiTests.ModuleIntegrationTests;

public class WeatherModuleIntegrationTests
{
    [Fact]
    public async Task ShouldServeTheGetWeatherRequest()
    {
        //Given
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        IConfigurationRoot config = builder.Configuration
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        WebApplication app = builder.Build();
        string port = "8080";
        Mock<IWeatherApiHttpClient> mockedWeatherApiClient = new();
        CurrentWeatherDto currentWeatherDto = new()
        {
            Country = "IE",
            TemperatureCelsius = 20.0,
            Description = "Cloudy",
            Icon = "//cdn.weatherapi.com/weather/64x64/day/116.png"
        };

        TimezoneDto timezoneDto = new()
        {
            LocalTimeEpoch = 1630000000,
            TimezoneId = "Europe/Dublin"
        };

        AstronomyDto astronomyDto = new()
        {
            Sunset = "2021-08-27T20:00:00",
            Sunrise = "2021-08-27T06:00:00"
        };
        mockedWeatherApiClient.Setup(w => w.GetCurrentWeatherAsync("Dublin")).ReturnsAsync(currentWeatherDto);
        mockedWeatherApiClient.Setup(w => w.GetTimezoneDtoAsync("Dublin")).ReturnsAsync(timezoneDto);
        mockedWeatherApiClient.Setup(w => w.GetAstronomyDtoAsync("Dublin")).ReturnsAsync(astronomyDto);
        await new WeatherModule()
            .WithHttpClient(mockedWeatherApiClient.Object)
            .WithAvailableCities(() => Task.FromResult<IEnumerable<string>>([ "Dublin" ]))
            .WithConfig(config)
            .WithRoutes("weather", app)
            .InitializeAsync();
        Task runningApp = app.RunAsync($"http://*:{port}");
        HttpClient client = new HttpClient();


        //When
        HttpResponseMessage response = await client.GetAsync($"http://localhost:{port}/weather/Dublin");

        //Then
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        await app.StopAsync();
    }    
}