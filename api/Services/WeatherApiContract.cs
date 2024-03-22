using System.Text.Json.Serialization;

namespace Api.Services;

public static class WeatherApiContract
{
    public class Location
    {
        public required string Country { get; set; }
        [JsonPropertyName("localtime_epoch")]
        public long LocaltimeEpoch { get; set; }
        [JsonPropertyName("tz_id")]
        public required string TzId { get; set; }
    }

    public class AstronomyResponse
    {
        public required AstronomyProp Astronomy { get; set; }

        public class AstronomyProp
        {
            public required AstroProp Astro { get; set; }

            public class AstroProp
            {
                public required string Sunrise { get; set; }
                public required string Sunset { get; set; }
            }
        }
    }

    public class CurrentWeatherResponse
    {
        public required CurrentProp Current { get; set; }
        public required Location Location { get; set; }

        public class CurrentProp
        {
            [JsonPropertyName("temp_c")]
            public double TempC { get; set; }
            public required ConditionProp Condition { get; set; }

            public class ConditionProp
            {
                public required string Text { get; set; }
                public required string Icon { get; set; }
            }
        }
    }

    public class TimezoneResponse
    {
        public required Location Location { get; set; }
    }
}