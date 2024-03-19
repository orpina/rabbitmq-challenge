using RabbitMQChallenge.Domain.Core.Abstractions;

namespace RabbitMQChallenge.Domain.Core.Interfaces
{
    public interface IMessageBus
    {
        void Publish<T>(T customEvent)
            where T : BaseEvent;

        void Subscribe<T, TH>() 
            where T : BaseEvent 
            where TH : IMessageBusHandler<T>;

        Task SendCommand<T>(T command)
            where T : BaseCommand;
    }
}
