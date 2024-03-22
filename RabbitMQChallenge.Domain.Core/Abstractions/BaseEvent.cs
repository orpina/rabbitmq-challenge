
namespace RabbitMQChallenge.Domain.Core.Abstractions
{
    public abstract class BaseEvent
    {
        public DateTime TimeStamp { get; set; }
        public required List<string> BusAudience { get; set; } = [];
    }
}
