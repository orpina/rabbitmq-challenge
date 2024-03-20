using RabbitMQChallenge.Mapping.Application.Services;

namespace RabbitMQChallenge.Mapping.Domain.BusHandlers
{
    public class GeoUpdateHandle : IMessageBusHandler
    {
        private readonly IGeoPointService _geoPointService;

        public GeoUpdateHandle(IGeoPointService geoPointService)
        {
            _geoPointService = geoPointService;
        }

        public Task ProcessMessage()
        {
            _geoPointService.AddGeoPoint(null);
            return Task.CompletedTask;
        }
    }
}
