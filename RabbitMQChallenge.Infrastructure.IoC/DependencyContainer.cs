﻿using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQChallenge.Domain.Core.Interfaces;
using RabbitMQChallenge.Infrastructure.Bus;
using RabbitMQChallenge.Mapping.Application.EventHandlers;
using RabbitMQChallenge.Mapping.Application.Events;
using RabbitMQChallenge.Mapping.Application.Services;
using RabbitMQChallenge.Tracking.Application.CommandHandlers;
using RabbitMQChallenge.Tracking.Application.Commands;
using RabbitMQChallenge.Tracking.Application.Services;

namespace RabbitMQChallenge.Infrastructure.IoC
{
    public class DependencyContainer
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, RabbitMQBus>(sp =>
            {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

                return new RabbitMQBus(
                    sp.GetService<IConfiguration>()!,
                    sp.GetService<IMediator>()!,
                    scopeFactory);
            });

            RegisterCommands(services);
            RegisterSubscriptions(services);

            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IGeoPointService, GeoPointService>();
        }

        private static void RegisterCommands(IServiceCollection services)
        {
            services.AddTransient<IRequestHandler<LocationUpdateCommand, bool>, LocationUpdateCommandHandler>();
        }

        private static void RegisterEvents(IServiceCollection services)
        {
            services.AddTransient<IMessageBusHandler<LocationUpdateEvent>, LocationUpdateEventHandler>();
        }

        private static void RegisterSubscriptions(IServiceCollection services)
        {
            services.AddTransient<LocationUpdateEventHandler>();
        }
    }
}
