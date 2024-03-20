using RabbitMQChallenge.Domain.Core.Interfaces;
using RabbitMQChallenge.Tracking.Application.Commands;
using RabbitMQChallenge.Tracking.Application.Models;

namespace RabbitMQChallenge.Tracking.Application.Services
{
    public class LocationService(IMessageBus bus) : ILocationService
    {
        private readonly IMessageBus _bus = bus;

        public bool IsValidUpdate(LocationUpdateRequest updateRequest)
        {
            return updateRequest is not null;
        }

        public void ProcessUpdate(LocationUpdateRequest updateRequest)
        {
            LocationUpdateCommand command = new () 
            {  
               DeviceId = updateRequest.DeviceId,
               Latitude =updateRequest.Latitude,
               Longitude =updateRequest.Longitude,
               TimeStamp = DateTime.UtcNow
            };

            _bus.SendCommand(command);
        }
    }
}
