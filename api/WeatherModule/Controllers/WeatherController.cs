using System.Text.Json;
using Api.WeatherModule.Models;
using Api.WeatherModule.Ports;
using Microsoft.AspNetCore.Mvc;

namespace Api.WeatherModule.Controllers
{
    public class WeatherController
    {
        private readonly IWeatherApiHttpClient _httpClient;
        private readonly IEnumerable<string> _availableCities;
        private readonly Action<LogLevel, string>? _log;

        public WeatherController(IWeatherApiHttpClient httpClient, IEnumerable<string> availableCities, Action<LogLevel, string>? log = null)
        {
            _httpClient = httpClient;
            _availableCities = availableCities;
            _log = log;
        }

        public async Task<WeatherOutput?> GetWeatherAsync(string city)
        {
            _log?.Invoke(LogLevel.Information, "City: " + city);
            _log?.Invoke(LogLevel.Information, "Available cities: " + string.Join(",", _availableCities));
            if (!_availableCities.Contains(city.ToLowerInvariant()))
            {
                return null;
            }
            
            CurrentWeatherDto currentWeather = await _httpClient.GetCurrentWeatherAsync(city);
            _log?.Invoke(LogLevel.Information, "Current weather: " + JsonSerializer.Serialize(currentWeather));
            TimezoneDto timezone = await _httpClient.GetTimezoneDtoAsync(city);
            _log?.Invoke(LogLevel.Information, "Timezone: " + JsonSerializer.Serialize(timezone));
            AstronomyDto astronomy = await _httpClient.GetAstronomyDtoAsync(city);
            _log?.Invoke(LogLevel.Information, "Astronomy: " + JsonSerializer.Serialize(astronomy));

            return new WeatherOutput
            {
                City = new City { Name = city },
                Country = currentWeather.Country,
                LocalTimeEpoch = timezone.LocalTimeEpoch,
                TimezoneId = timezone.TimezoneId,
                Sunset = astronomy.Sunset,
                Sunrise = astronomy.Sunrise,
                TemperatureCelsius = currentWeather.TemperatureCelsius,
                Description = new WeatherDescription
                {
                    Text = currentWeather.Description,
                    Icon = currentWeather.Icon
                }
            };
        }
    }
}