using System;

namespace Api.WeatherModule.Models;

public class WeatherOutput
{
    public required City City { get; set; }
    public required string Country { get; set; }
    public required long LocalTimeEpoch { get; set; }
    public required string TimezoneId { get; set; }
    public required string Sunset { get; set; }
    public required string Sunrise { get; set; }
    public required double TemperatureCelsius { get; set; }
    public required WeatherDescription Description { get; set; }
}