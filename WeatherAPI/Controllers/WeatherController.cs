using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [HttpGet("{cityNode}")]
        public IActionResult GetWeather(string cityNode)
        {
            return Ok( new
            {
                temperatureC = 25,
                Description = "Sunny",
                city = cityNode
            });
        }
    }
}
