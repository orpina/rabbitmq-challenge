using RabbitMQChallenge.Analytics.Application.Events;
using RabbitMQChallenge.Analytics.Application.Models;
using RabbitMQChallenge.Analytics.Application.Services;
using RabbitMQChallenge.Domain.Core.Interfaces;

namespace RabbitMQChallenge.Analytics.Application.EventHandlers
{
    public class LocationUpdateEventHandler(IRouteService routeService) : IMessageBusHandler<LocationUpdateEvent>
    {
        private readonly IRouteService _routervice = routeService;

        public Task Handle(LocationUpdateEvent customEvent)
        {
            _routervice.UpdateDeviceRoute(customEvent.DeviceId, new DeviceLocationVM()
            {
                Latitude = customEvent.Latitude,
                Longitude = customEvent.Longitude,
                TimeStamp = customEvent.TimeStamp,
            });

            return Task.CompletedTask;
        }
    }
}
