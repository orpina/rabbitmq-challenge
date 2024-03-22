namespace RabbitMQChallenge.Analytics.Application.Models
{
    public class DeviceRouteVM
    {
        public required string DeviceId { get; set; }

        public required List<DeviceLocationVM> Routes { get; set; }
    }
}
