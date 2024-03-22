using MediatR;

namespace RabbitMQChallenge.Domain.Core.Abstractions
{
    public abstract class BaseMessage : IRequest<bool>
    {
        public string MessageType { get; protected set; }

        protected BaseMessage()
        {
            MessageType = GetType().Name;
        }
    }
}
