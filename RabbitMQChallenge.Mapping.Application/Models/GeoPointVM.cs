namespace RabbitMQChallenge.Mapping.Application.Models
{
    public class GeoPointVM
    {
        public required string DeviceId { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime TimeStamp { get; set; }

        public string? Address { get; set; }
    }
}
