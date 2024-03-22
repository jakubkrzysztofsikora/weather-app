namespace Api.WeatherModule.Ports;

public class CurrentWeatherDto
{
    public required string Country { get; set; }
    public required double TemperatureCelsius { get; set; }
    public required string Description { get; set; }
    public required string Icon { get; set; }
}

public class TimezoneDto
{
    public required long LocalTimeEpoch { get; set; }
    public required string TimezoneId { get; set; }
}

public class AstronomyDto
{
    public required string Sunset { get; set; }
    public required string Sunrise { get; set; }
}