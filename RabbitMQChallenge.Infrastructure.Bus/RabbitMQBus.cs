using RabbitMQ.Client;
using RabbitMQChallenge.Domain.Core.Bus;
using System.Text;

namespace RabbitMQChallenge.Infrastructure.Bus
{
    public class RabbitMQBus : IBus
    {
        public void Publish(string message) 
        { 
            ConnectionFactory factory = new ()
            {
                HostName = "localhost",
            };

            using IConnection conn = factory.CreateConnection();
            using IModel model = conn.CreateModel();

            string queueName = "test";
            byte[] body = Encoding.UTF8.GetBytes(message);

            model.QueueDeclare(queueName, false, false, false, null);
            model.BasicPublish(string.Empty, queueName, null, body);
        }
    }
}
