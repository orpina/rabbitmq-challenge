using RabbitMQChallenge.Domain.Core.Interfaces;
using RabbitMQChallenge.Mapping.Application.Events;
using RabbitMQChallenge.Mapping.Application.Models;
using RabbitMQChallenge.Mapping.Application.Services;

namespace RabbitMQChallenge.Mapping.Application.EventHandlers
{
    public class LocationUpdateEventHandler(IGeoPointService geoPointService) : IMessageBusHandler<LocationUpdateEvent>
    {
        private readonly IGeoPointService _geoPointService = geoPointService;

        public Task Handle(LocationUpdateEvent customEvent)
        {
            _geoPointService.AddGeoPoint(new GeoPointVM()
            {
                DeviceId = customEvent.DeviceId,
                Latitude = customEvent.Latitude,
                Longitude = customEvent.Longitude,
                TimeStamp = customEvent.TimeStamp,
                Address = string.Empty
            });

            return Task.CompletedTask;
        }
    }
}
