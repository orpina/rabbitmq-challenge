namespace RabbitMQChallenge.Domain.Core.Abstractions
{
    public abstract class BaseCommand : BaseMessage
    {
        public DateTime TimeStamp { get; set; }

    }
}
