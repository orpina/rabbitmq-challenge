using RabbitMQChallenge.Domain.Core.Abstractions;

namespace RabbitMQChallenge.Mapping.Application.Events
{
    public class LocationUpdateEvent(string deviceId, double latitude, double longitude) : BaseEvent
    {
        public string DeviceId { get; private set; } = deviceId;

        public double Latitude { get; private set; } = latitude;

        public double Longitude { get; private set; } = longitude;
    }
}
