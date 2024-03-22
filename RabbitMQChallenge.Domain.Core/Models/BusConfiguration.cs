namespace RabbitMQChallenge.Domain.Core.Models
{
    public record BusConfiguration
    {
        public required string HostName { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required bool IsAutoAck { get; set; }
    }
}
