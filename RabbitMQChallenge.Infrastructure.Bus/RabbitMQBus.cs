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
        private readonly JsonSerializerOptions _jsonSerializerOptions = new () 
        { 
            PropertyNameCaseInsensitive = true
        };

        public void Publish<T>(T customEvent)
            where T : BaseEvent
        {
            if (customEvent is null || customEvent.BusAudience is null || customEvent.BusAudience.Count == 0)
            {
                return;
            }

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

            customEvent.BusAudience.ForEach((string queueName) =>
            {
                model.QueueDeclare(queueName, false, false, false, null);
                model.BasicPublish(string.Empty, queueName, null, body);
            });
        }

        public void Subscribe<T, R>(string queueName)
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

            bool existsHandler = _handlers.ContainsKey(eventName);
            if (!existsHandler)
            {
                _handlers.Add(eventName, []);
            }

            if (!_handlers[eventName].Any(h => h.GetType() == handlerType))
            {
                _handlers[eventName].Add(handlerType);
            }

            Initialize<T>(queueName);
        }

        public Task SendCommand<T>(T command) where T : BaseCommand
        {
            return _mediator.Send(command);
        }

        private void Initialize<T>(string queueName)
        {
            ConnectionFactory factory = new()
            {
                HostName = _busConfig.HostName,
                UserName = _busConfig.UserName,
                Password = _busConfig.Password,
                DispatchConsumersAsync = true,
            };

            IConnection conn = factory.CreateConnection();
            IModel model = conn.CreateModel();

            model.QueueDeclare(queueName, false, false, false, null);

            AsyncEventingBasicConsumer consumer = new(model);

            consumer.Received += Consumer_Received<T>;

            model.BasicConsume(queueName, _busConfig.IsAutoAck, consumer);
        }

        private async Task Consumer_Received<T>(object sender, BasicDeliverEventArgs e)
        {
            string eventName = typeof(T).Name;
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

        private async Task ProcessEvent(string eventName, string message)
        {
            bool exists = _handlers.ContainsKey(eventName);

            if (exists)
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();

                List<Type> subscriptions = _handlers[eventName];

                foreach (var subscription in subscriptions)
                {
                    object handler = scope.ServiceProvider.GetService(subscription)!;

                    if (handler is null)
                    {
                        continue;
                    }

                    Type eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName)!;

                    object customEvent = JsonSerializer.Deserialize(message, eventType, _jsonSerializerOptions)!;

                    Type concreteType = typeof(IMessageBusHandler<>).MakeGenericType(eventType)!;

                    await (Task)concreteType.GetMethod("Handle")?.Invoke(handler, [customEvent])!;
                }
            }
        }
    }
}
