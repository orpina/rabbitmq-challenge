namespace RabbitMQChallenge.Domain.Core.Bus
{
    public interface IBus
    {
        void Publish<T>(T payload, string queueName) where T : class;
    }
}
