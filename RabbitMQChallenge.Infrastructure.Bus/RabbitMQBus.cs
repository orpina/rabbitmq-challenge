using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQChallenge.Domain.Core.Bus;
using System.Text;

namespace RabbitMQChallenge.Infrastructure.Bus
{
    public class RabbitMQBus(IConfiguration config) : IBus
    {
        private readonly IConfiguration _config = config;
        private readonly Dictionary<string, List<Type>> _handlers = [];

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

        public void Subscribe<T>(string queueName)
        {
            Type handlerType = typeof(T);

            bool existsQueueName = _handlers.ContainsKey(queueName);

            if (!existsQueueName)
            {
                _handlers.Add(queueName, []);
            }

            if (!_handlers[queueName].Any(h => h.GetType() == handlerType))
            {
                _handlers[queueName].Add(handlerType);
            }

            Initialize<T>(queueName);
        }

        private void Initialize<T>(string queueName)
        {
            ConnectionFactory factory = new()
            {
                HostName = _config["BusConfig:HostName"],
                DispatchConsumersAsync = true,
            };

            IConnection conn = factory.CreateConnection();
            IModel model = conn.CreateModel();

            model.QueueDeclare(queueName, false, false, false, null);

            AsyncEventingBasicConsumer consumer = new(model);

            //consumer.Received += Consumer_Received;
            consumer.Received += async (model, e) =>
            {
                string queueName = e.RoutingKey;
                var message = Encoding.UTF8.GetString(e.Body.Span);

                try
                {
                    await ProcessEvent(queueName, message, typeof(T)).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    throw;
                }
            };

            model.BasicConsume(queueName, true, consumer);
        }

        //private async Task Consumer_Received(object sender, BasicDeliverEventArgs e, Type handlerType)
        //{
        //    string queueName = e.RoutingKey;
        //    var message = Encoding.UTF8.GetString(e.Body.Span);

        //    try
        //    {
        //        await ProcessEvent(queueName, message).ConfigureAwait(false);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        private async Task ProcessEvent(string queueName, string message, Type handlerType)
        {
            bool exists = _handlers.ContainsKey(queueName);

            if (exists)
            {
                List<Type> subscriptions = _handlers[queueName];

                foreach (var subscription in subscriptions)
                {
                    object handler = Activator.CreateInstance(subscription)!;

                    if (handler is null)
                    {
                        continue;
                    }

                    //Type eventType = _eventTypes.SingleOrDefault(t => t.Name == queueName)!;                    

                    Type type = typeof(IBusMessageHandler).MakeGenericType(handlerType)!;

                    object payload = JsonConvert.DeserializeObject(message)!;

                    await (Task)type.GetMethod("ProcessMessage")?.Invoke(handler, [payload])!;
                }
            }
        }
    }
}
