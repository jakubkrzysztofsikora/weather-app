using Xunit;
using Microsoft.AspNetCore.Mvc;
using Api.WeatherModule.Controllers;
using Api.WeatherModule.Models;
using Api.WeatherModule.Ports;
using ApiTests.WeatherModuleTests.Mocks;

namespace ApiTests.WeatherModuleTests
{
    public class WeatherGetTests
    {
        [Fact]
        public async void ShouldReturnCombinedWeatherInformationFromDifferentWeatherApiEndpoints_WhenCityIsSupported()
        {
            // Given
            string city = "dublin";
            IWeatherApiHttpClient mockedWeatherApiClient = new MockedWeatherApiClient();
            WeatherController weatherController = new WeatherController(mockedWeatherApiClient, [city]);
            WeatherOutput expected = new WeatherOutput
            {
                City = new City { Name = "dublin" },
                Country = "IE",
                LocalTimeEpoch = 1630000000,
                TimezoneId = "Europe/Dublin",
                Sunset = "2021-08-27T20:00:00",
                Sunrise = "2021-08-27T06:00:00",
                TemperatureCelsius = 20.0,
                Description = new WeatherDescription
                {
                    Text = "Cloudy",
                    Icon = "//cdn.weatherapi.com/weather/64x64/day/116.png"
                }
            };

            // When
            WeatherOutput result = await weatherController.GetWeatherAsync(city);

            // Then
            Assert.Equivalent(expected, result);
        }

        [Fact]
        public async void ShouldReturnNotFound_WhenCityIsNotSupported()
        {
            // Given
            string city = "Unknown";
            IWeatherApiHttpClient mockedWeatherApiClient = new MockedWeatherApiClient();
            WeatherController weatherController = new WeatherController(mockedWeatherApiClient, []);

            // When
            WeatherOutput result = await weatherController.GetWeatherAsync(city);

            // Then
            Assert.Null(result);
        }
    }
}
