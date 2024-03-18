using Microsoft.AspNetCore.Mvc;
using RabbitMQChallenge.Tracking.Application.Models;
using RabbitMQChallenge.Tracking.Application.Services;

namespace RabbitMQChallenge.Tracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController(ILocationService locationService) : ControllerBase
    {
        private readonly ILocationService _locationService = locationService;

        [HttpPost]
        public IActionResult UpdateLocation([FromBody] LocationUpdateRequest updateRequest)
        {
            if (!_locationService.IsValidUpdate(updateRequest))
            {
                return BadRequest();
            }

            _locationService.ProcessUpdate(updateRequest);

            return Ok(updateRequest);
        }
    }
}
