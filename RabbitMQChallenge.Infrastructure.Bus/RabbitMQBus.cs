using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQChallenge.Domain.Core.Abstractions;
using RabbitMQChallenge.Domain.Core.Interfaces;
using RabbitMQChallenge.Domain.Core.Models;
using System.Text;
using System.Text.Json;

namespace RabbitMQChallenge.Infrastructure.Bus
{
    public class RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory, BusConfiguration busConfig) : IMessageBus
    {
        private readonly IMediator _mediator = mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly BusConfiguration _busConfig = busConfig;
        private readonly Dictionary<string, List<Type>> _handlers = [];
        private readonly List<Type> _eventTypes = [];

        public void Publish<T>(T customEvent)
            where T : BaseEvent
        {
            ConnectionFactory factory = new ()
            {
                HostName = _busConfig.HostName,
                UserName = _busConfig.UserName,
                Password = _busConfig.Password,
            };

            string eventName = customEvent.GetType().Name;
            string message = JsonSerializer.Serialize(customEvent);

            using IConnection conn = factory.CreateConnection();
            using IModel model = conn.CreateModel();

            byte[] body = Encoding.UTF8.GetBytes(message);

            model.QueueDeclare(eventName, false, false, false, null);
            model.BasicPublish(string.Empty, eventName, null, body);
        }

        public void Subscribe<T, R>()
            where T : BaseEvent
            where R : IMessageBusHandler<T>
        {
            string eventName = typeof(T).Name;
            Type handlerType = typeof(R);

            bool existsEventType = _eventTypes.Contains(typeof(T));
            if (!existsEventType)
            {
                _eventTypes.Add(typeof(T));
            }

            bool existsQueueName = _handlers.ContainsKey(eventName);
            if (!existsQueueName)
            {
                _handlers.Add(eventName, []);
            }

            if (!_handlers[eventName].Any(h => h.GetType() == handlerType))
            {
                _handlers[eventName].Add(handlerType);
            }

            Initialize<T>();
        }

        public Task SendCommand<T>(T command) where T : BaseCommand
        {
            return _mediator.Send(command);
        }

        private void Initialize<T>()
        {
            ConnectionFactory factory = new()
            {
                HostName = _busConfig.HostName,
                UserName = _busConfig.UserName,
                Password = _busConfig.Password,
                DispatchConsumersAsync = true,
            };

            string eventName = typeof(T).Name;

            IConnection conn = factory.CreateConnection();
            IModel model = conn.CreateModel();

            model.QueueDeclare(eventName, false, false, false, null);

            AsyncEventingBasicConsumer consumer = new(model);

            consumer.Received += Consumer_Received;

            model.BasicConsume(eventName, _busConfig.IsAutoAck, consumer);
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string eventName = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                await ProcessEvent(eventName, message).ConfigureAwait(false);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task ProcessEvent(string queueName, string message)
        {
            bool exists = _handlers.ContainsKey(queueName);

            if (exists)
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();

                List<Type> subscriptions = _handlers[queueName];

                foreach (var subscription in subscriptions)
                {
                    object handler = scope.ServiceProvider.GetService(subscription)!;

                    if (handler is null)
                    {
                        continue;
                    }

                    Type eventType = _eventTypes.SingleOrDefault(t => t.Name == queueName)!;

                    object customEvent = JsonSerializer.Deserialize(message, eventType)!;

                    Type concreteType = typeof(IMessageBusHandler<>).MakeGenericType(eventType)!;

                    await (Task)concreteType.GetMethod("Handle")?.Invoke(handler, [customEvent])!;
                }
            }
        }
    }
}
