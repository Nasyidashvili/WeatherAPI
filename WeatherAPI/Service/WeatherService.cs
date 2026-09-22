using WeatherAPI.Interface;
using WeatherAPI.DTO;
using WeatherAPI.Model;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace WeatherAPI.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string? baseUrl;
        private readonly string? apiKey;
        private readonly IOptions<WeatherApiSettings> _options;

        public WeatherService(HttpClient httpClient, IOptions<WeatherApiSettings> options)
        {
            _httpClient = httpClient;
            _options = options;
            baseUrl = options.Value.BaseUrl;
            apiKey = options.Value.ApiKey;
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<WeatherDto> GetWeatherByLocation(string? location)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{location}/{DateTime.Now.ToString("yyyy-MM-dd")}?unitGroup=us&include=days&key={apiKey}&contentType=json");
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

                return weatherData;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error fetching weather data", ex);
            }
        }
    }
}
