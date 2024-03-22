using RabbitMQChallenge.Domain.Core.Abstractions;

namespace RabbitMQChallenge.Tracking.Application.Events
{
    public class LocationUpdateEvent : BaseEvent
    {
        public required string DeviceId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
