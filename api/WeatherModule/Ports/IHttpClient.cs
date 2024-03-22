namespace Api.WeatherModule.Ports
{
    public interface IWeatherApiHttpClient
    {
        Task<CurrentWeatherDto> GetCurrentWeatherAsync(string city);
        Task<TimezoneDto> GetTimezoneDtoAsync(string city);
        Task<AstronomyDto> GetAstronomyDtoAsync(string city);
    }
}