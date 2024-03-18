using Microsoft.Extensions.Configuration;
using RabbitMQChallenge.Domain.Core.Bus;
using RabbitMQChallenge.Tracking.Application.Models;

namespace RabbitMQChallenge.Tracking.Application.Services
{
    public class LocationService(IBus bus, IConfiguration config) : ILocationService
    {
        private readonly IBus _bus = bus;
        private readonly IConfiguration _config = config;

        public bool IsValidUpdate(LocationUpdateRequest updateRequest)
        {
            return updateRequest is not null;
        }

        public void ProcessUpdate(LocationUpdateRequest updateRequest)
        {
            _bus.Publish(updateRequest, _config["BusConfig:GeoUpdateQueue"]!);
        }
    }
}
