namespace RabbitMQChallenge.Domain.Core.Bus
{
    public interface IBus
    {
        void Publish(string message);
    }
}
