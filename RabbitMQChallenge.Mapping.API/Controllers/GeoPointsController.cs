using Microsoft.AspNetCore.Mvc;
using RabbitMQChallenge.Mapping.Application.Models;
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
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return BadRequest($"Invalid { nameof(deviceId) }");
            }

            var results = _geoPointService.GetGeoPoints(deviceId);

            if (results is null || !results.Any())
            {
                return Ok(new
                {
                    total = 0,
                    results = Enumerable.Empty<GeoPointVM>(),
                    message = $"No Geo Points registered for device { nameof(deviceId) }"
                });
            }

            return Ok(new
            {
                total = results.Count(),
                results,
                message = string.Empty
            });
        }
    }
}
