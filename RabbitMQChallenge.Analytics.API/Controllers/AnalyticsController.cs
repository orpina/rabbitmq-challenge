using Microsoft.AspNetCore.Mvc;
using RabbitMQChallenge.Analytics.Application.Models;
using RabbitMQChallenge.Analytics.Application.Services;

namespace RabbitMQChallenge.Analytics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController(IRouteService routeService) : ControllerBase
    {
        private readonly IRouteService _routeService = routeService;

        [HttpGet]
        [Route("GetById/{deviceId}")]
        public IActionResult Get(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                return BadRequest($"Invalid {nameof(deviceId)}");
            }

            var result = _routeService.GetDeviceRoute(deviceId);

            if (result is null)
            {
                return Ok(new {
                    deviceId,
                    success = false,
                    message = $"No device registered with requested Id" 
                });
            }

            return Ok(new
            {
                deviceId,
                success = true,
                routes = new {
                    total = result.Routes.Count,
                    deviceRoutes = result.Routes
                },
                message = string.Empty
            });
        }
    }
}
