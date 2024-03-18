using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQChallenge.Domain.Core.Bus;
using System.Text;

namespace RabbitMQChallenge.Infrastructure.Bus
{
    public class RabbitMQBus(IConfiguration config) : IBus
    {
        private readonly IConfiguration _config = config;

        public void Publish<T>(T payload, string queueName) where T : class
        {             
            ConnectionFactory factory = new ()
            {
                HostName = _config["BusConfig:HostName"],
            };

            string message = JsonConvert.SerializeObject(payload);

            using IConnection conn = factory.CreateConnection();
            using IModel model = conn.CreateModel();

            byte[] body = Encoding.UTF8.GetBytes(message);

            model.QueueDeclare(queueName, false, false, false, null);
            model.BasicPublish(string.Empty, queueName, null, body);
        }
    }
}
