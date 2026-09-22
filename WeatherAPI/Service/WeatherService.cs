using WeatherAPI.Interface;
using WeatherAPI.DTO;
using WeatherAPI.Model;  
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace WeatherAPI.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string? baseUrl;
        private readonly string? apiKey;
        private readonly ICacheService _cache;

        public WeatherService(HttpClient httpClient, IOptions<WeatherApiSettings> options, ICacheService cache)
        {
            _httpClient = httpClient;
            baseUrl = options.Value.BaseUrl;
            apiKey = options.Value.ApiKey;
            _httpClient.BaseAddress = new Uri(baseUrl);
            _cache = cache;
        }   

        public async Task<WeatherDto> GetWeatherByLocation(string? location)
        {
            string cacheKey = $"weather:{location?.ToLower()}";

            var cached = await _cache.GetAsync<WeatherDto>(cacheKey);
            if (cached != null)
            {
                return cached;
            }

            try
            {
                var response = await _httpClient.GetAsync($"{location}/{DateTime.Now.ToString("yyyy-MM-dd")}?unitGroup=metric&include=days&key={apiKey}&contentType=json");
                response.EnsureSuccessStatusCode();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var raw = await response.Content.ReadFromJsonAsync<VisualCrossingResponse>(options);
                var today = raw?.Days?.FirstOrDefault();

                if (today == null)
                {
                    throw new ApplicationException("No weather data available for today.");
                }

                var weatherData = new WeatherDto
                {
                    DateTime = today.Datetime,
                    TemperatureC = today.Temp,
                    Humidity = today.Humidity
                };

                await _cache.SetAsync(cacheKey, weatherData, TimeSpan.FromMinutes(30));

                return weatherData;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching weather data", ex);
            }
        }
    }
}
