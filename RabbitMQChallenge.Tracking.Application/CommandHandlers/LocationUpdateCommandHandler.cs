using MediatR;
using RabbitMQChallenge.Domain.Core.Interfaces;
using RabbitMQChallenge.Tracking.Application.Commands;
using RabbitMQChallenge.Tracking.Application.Events;

namespace RabbitMQChallenge.Tracking.Application.CommandHandlers
{
    public class LocationUpdateCommandHandler(IMessageBus bus) : IRequestHandler<LocationUpdateCommand, bool>
    {
        private readonly IMessageBus _bus = bus;
        private readonly string _analyticsQueue = "analytics-queue";
        private readonly string _mappingQueue = "mapping-queue";
        public Task<bool> Handle(LocationUpdateCommand command, CancellationToken cancellationToken)
        {
            LocationUpdateEvent customEvent = new ()
            {
                DeviceId = command.DeviceId,
                Latitude = command.Latitude,
                Longitude = command.Longitude,
                TimeStamp = command.TimeStamp,
                BusAudience = [_analyticsQueue, _mappingQueue]
            };

            _bus.Publish(customEvent);

            return Task.FromResult(true);
        }
    }
}
