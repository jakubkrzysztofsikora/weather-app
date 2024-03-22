using System.Threading.Tasks;
using Api.WeatherModule.Ports;

namespace ApiTests.WeatherModuleTests.Mocks
{
    public class MockedWeatherApiClient : IWeatherApiHttpClient
    {
        public Task<CurrentWeatherDto> GetCurrentWeatherAsync(string city)
        {
            return Task.FromResult(new CurrentWeatherDto
            {
                Country = "IE",
                TemperatureCelsius = 20.0,
                Description = "Cloudy",
                Icon = "//cdn.weatherapi.com/weather/64x64/day/116.png"
            });
        }

        public Task<TimezoneDto> GetTimezoneDtoAsync(string city)
        {
            return Task.FromResult(new TimezoneDto
            {
                LocalTimeEpoch = 1630000000,
                TimezoneId = "Europe/Dublin"
            });
        }

        public Task<AstronomyDto> GetAstronomyDtoAsync(string city)
        {
            return Task.FromResult(new AstronomyDto
            {
                Sunset = "2021-08-27T20:00:00",
                Sunrise = "2021-08-27T06:00:00"
            });
        }
    }
}