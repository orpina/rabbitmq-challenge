using RabbitMQChallenge.Domain.Core.Abstractions;

namespace RabbitMQChallenge.Domain.Core.Interfaces
{
    public interface IMessageBusHandler<in TEvent> : IMessageBusHandler where TEvent : BaseEvent
    {
        Task Handle(TEvent @event);
    }

    public interface IMessageBusHandler
    {
    }
}
