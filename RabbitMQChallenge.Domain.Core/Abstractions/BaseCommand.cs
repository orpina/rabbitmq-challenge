using RabbitMQChallenge.Domain.Core.Abstractions;

namespace RabbitMQChallenge.Domain.Core.Abstractions
{
    public abstract class BaseCommand : BaseMessage
    {
        public DateTime TimeStamp { get; protected set; }

        protected BaseCommand()
        {
            TimeStamp = DateTime.Now;
        }
    }
}
