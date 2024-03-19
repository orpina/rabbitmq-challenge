using MediatR;
using RabbitMQChallenge.Domain.Core.Interfaces;
using RabbitMQChallenge.Tracking.Application.Commands;
using RabbitMQChallenge.Tracking.Application.Events;

namespace RabbitMQChallenge.Tracking.Application.CommandHandlers
{
    public class LocationUpdateCommandHandler(IMessageBus bus) : IRequestHandler<LocationUpdateCommand, bool>
    {
        private readonly IMessageBus _bus = bus;

        public Task<bool> Handle(LocationUpdateCommand command, CancellationToken cancellationToken)
        {
            LocationUpdateEvent customEvent = new (command.DeviceId, command.Latitude, command.Longitude);
            
            _bus.Publish(customEvent);

            return Task.FromResult(true);
        }
    }
}
