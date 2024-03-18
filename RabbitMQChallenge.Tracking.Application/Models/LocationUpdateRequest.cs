using System.ComponentModel.DataAnnotations;

namespace RabbitMQChallenge.Tracking.Application.Models
{
    public class LocationUpdateRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = $"Invalid {nameof(DeviceId)} value")]
        public required string DeviceId { get; set; }

        [Range(double.Epsilon, double.MaxValue, ErrorMessage = $"Invalid {nameof(Latitude)} value")]
        public double Latitude { get; set; }

        [Range(double.Epsilon, double.MaxValue, ErrorMessage = $"Invalid {nameof(Longitude)} value")]
        public double Longitude { get; set; }
    }
}
