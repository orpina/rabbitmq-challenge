using Microsoft.Extensions.DependencyInjection;
using RabbitMQChallenge.Domain.Core.Bus;
using RabbitMQChallenge.Infrastructure.Bus;
using RabbitMQChallenge.Mapping.Application.Services;
using RabbitMQChallenge.Tracking.Application.Services;

namespace RabbitMQChallenge.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IBus, RabbitMQBus>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IGeoPointService, GeoPointService>();
        }
    }
}
