using Microsoft.AspNetCore.Mvc;
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
            var result = _routeService.GetDeviceRoute(deviceId);

            return Ok(result);
        }
    }
}
