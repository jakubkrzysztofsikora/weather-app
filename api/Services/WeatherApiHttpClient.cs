using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Api.WeatherModule.Ports;
using static Api.Services.WeatherApiContract;

namespace Api.Services;

public class WeatherApiHttpClient : IWeatherApiHttpClient
{
    private readonly string _apiKey;
    private readonly string _apiKeyHeaderKey;
    private readonly HttpClient _httpClient;
    private readonly Action<LogLevel, string>? _log;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public WeatherApiHttpClient(string apiKey, string apiKeyHeaderKey, Action<LogLevel, string>? log = null)
    {
        _apiKey = apiKey;
        _apiKeyHeaderKey = apiKeyHeaderKey;
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://weatherapi-com.p.rapidapi.com"),
            DefaultRequestHeaders =
            {
                { _apiKeyHeaderKey, _apiKey }
            }
        };
        _log = log;

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        _jsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseToCamelCaseNamingPolicy();
    }

    public async Task<AstronomyDto> GetAstronomyDtoAsync(string city)
    {
        city = SanatizeCityName(city);
        try
        {
            _log?.Invoke(LogLevel.Information, "Getting astronomy for city: " + city);
            AstronomyResponse? response = await JsonSerializer.DeserializeAsync<AstronomyResponse>(
                await _httpClient.GetStreamAsync($"/astronomy.json?q={city}"), _jsonSerializerOptions) ?? throw new Exception("Failed to deserialize response");
            _log?.Invoke(LogLevel.Information, "Astronomy response: " + JsonSerializer.Serialize(response, _jsonSerializerOptions));
            return new AstronomyDto
            {
                Sunrise = response.Astronomy.Astro.Sunrise,
                Sunset = response.Astronomy.Astro.Sunset
            };
        }
        catch (Exception e)
        {
            _log?.Invoke(LogLevel.Error, e.Message);
            throw;
        }
    }

    public async Task<CurrentWeatherDto> GetCurrentWeatherAsync(string city)
    {
        city = SanatizeCityName(city);
        try
        {
            _log?.Invoke(LogLevel.Information, "Getting current weather for city: " + city);
            string rawResponse = await _httpClient.GetStringAsync($"/current.json?q={city}");
            _log?.Invoke(LogLevel.Information, "Raw Current Weather response: " + rawResponse);
            CurrentWeatherResponse? response = JsonSerializer.Deserialize<CurrentWeatherResponse>(rawResponse, _jsonSerializerOptions) ?? throw new Exception("Failed to deserialize response");
            _log?.Invoke(LogLevel.Information, "Current Weather response: " + JsonSerializer.Serialize(response, _jsonSerializerOptions));
            return new CurrentWeatherDto
            {
                Country = response.Location.Country,
                TemperatureCelsius = response.Current.TempC,
                Description = response.Current.Condition.Text,
                Icon = response.Current.Condition.Icon
            };
        }
        catch (Exception e)
        {
            _log?.Invoke(LogLevel.Error, e.Message);
            throw;
        }
    }

    public async Task<TimezoneDto> GetTimezoneDtoAsync(string city)
    {
        city = SanatizeCityName(city);
        try
        {
            _log?.Invoke(LogLevel.Information, "Getting timezone for city: " + city);
            TimezoneResponse? response = JsonSerializer.Deserialize<TimezoneResponse>(
                await _httpClient.GetStringAsync($"/timezone.json?q={city}"), _jsonSerializerOptions) ?? throw new Exception("Failed to deserialize response");
            _log?.Invoke(LogLevel.Information, "Timezone response: " + JsonSerializer.Serialize(response, _jsonSerializerOptions));

            return new TimezoneDto
            {
                LocalTimeEpoch = response.Location.LocaltimeEpoch,
                TimezoneId = response.Location.TzId
            };
        }
        catch (Exception e)
        {
            _log?.Invoke(LogLevel.Error, e.Message);
            throw;
        }
    }

    public class SnakeCaseToCamelCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) =>
            string.Concat(name.Split('_').Select((word, index) =>
                index > 0 ? char.ToUpper(word[0]) + word.Substring(1) : word.ToLower()));
    }

    private string SanatizeCityName(string city)
    {
        return Regex.Replace(city, @"[^A-Za-z]+", "");
    }
}