using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.DTO;
using WeatherAPI.Interface;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{location}")]
        public async Task<ActionResult<WeatherDto>> GetWeather(string? location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return BadRequest("City/location is required.");
            }
                

            var weather = await _weatherService.GetWeatherByLocation(location);
            return Ok(weather);
        }
    }
}
