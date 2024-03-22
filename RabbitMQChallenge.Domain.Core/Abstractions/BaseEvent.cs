
namespace RabbitMQChallenge.Domain.Core.Abstractions
{
    public abstract class BaseEvent
    {
        public DateTime TimeStamp { get; set; }
        public List<string>? BusAudience { get; set; } = [];
    }
}
