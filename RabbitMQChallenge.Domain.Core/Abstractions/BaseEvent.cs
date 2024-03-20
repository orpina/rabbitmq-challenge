
namespace RabbitMQChallenge.Domain.Core.Abstractions
{
    public abstract class BaseEvent
    {
        public DateTime TimeStamp { get; set; }
    }
}
