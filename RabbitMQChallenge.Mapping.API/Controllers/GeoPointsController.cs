using Microsoft.AspNetCore.Mvc;
using RabbitMQChallenge.Mapping.Application.Services;

namespace RabbitMQChallenge.Mapping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoPointsController(IGeoPointService geoPointService) : ControllerBase
    {
        private readonly IGeoPointService _geoPointService = geoPointService;

        [HttpGet]
        [Route("GetById/{deviceId}")]
        public IActionResult Get(string deviceId)
        {
            return Ok(_geoPointService.GetGeoPoints(deviceId));
        }
    }
}
