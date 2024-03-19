using RabbitMQChallenge.Domain.Core.Abstractions;

namespace RabbitMQChallenge.Tracking.Application.Commands
{
    public class LocationUpdateCommand : BaseCommand
    {
        public required string DeviceId { get; set; }

        public required double Latitude { get; set; }

        public required double Longitude { get; set; }
    }
}
