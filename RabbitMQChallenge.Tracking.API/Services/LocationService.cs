using RabbitMQChallenge.Domain.Core.Bus;
using RabbitMQChallenge.Tracking.API.Models;

namespace RabbitMQChallenge.Tracking.API.Services
{
    public class LocationService(IBus bus) : ILocationService
    {
        private readonly IBus _bus = bus;

        public bool IsValidUpdate(LocationUpdateRequest updateRequest)
        {
            return updateRequest is not null;
        }

        public void ProcessUpdate(LocationUpdateRequest updateRequest)
        {
            _bus.Publish($"Message created { updateRequest.DeviceId} ");
        }
    }
}
