using WeatherAPI.DTO;

namespace WeatherAPI.Interface
{
    public interface IWeatherService 
    {
        public Task<WeatherDto> GetWeatherByLocation(string? location);
    }
}
