using MediatR;
using RabbitMQChallenge.Domain.Core.Interfaces;
using RabbitMQChallenge.Domain.Core.Models;
using RabbitMQChallenge.Infrastructure.Bus;
using RabbitMQChallenge.Tracking.Application.CommandHandlers;
using RabbitMQChallenge.Tracking.Application.Commands;
using RabbitMQChallenge.Tracking.Application.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
RegisterServices();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void RegisterServices()
{
    builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    builder.Services.AddSingleton<IMessageBus, RabbitMQBus>(sp =>
    {
        var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();

        return new RabbitMQBus(
            sp.GetService<IMediator>()!,
            scopeFactory,
            GetBusConfiguration());
    });

    builder.Services.AddTransient<ILocationService, LocationService>();

    builder.Services.AddTransient<IRequestHandler<LocationUpdateCommand, bool>, LocationUpdateCommandHandler>();


    BusConfiguration GetBusConfiguration()
    {
        _ = bool.TryParse(builder.Configuration["BusConfig:UseLocalService"], out bool useLocalService);
        _ = bool.TryParse(builder.Configuration["BusConfig:IsAutoAck"], out bool isAutoAck);
        string serviceType = useLocalService ? "Local" : "Container";

        return new()
        {
            HostName = builder.Configuration[$"BusConfig:{serviceType}HostName"]!,
            UserName = builder.Configuration[$"BusConfig:{serviceType}UserName"]!,
            Password = builder.Configuration[$"BusConfig:{serviceType}Password"]!,
            IsAutoAck = isAutoAck
        };
    }
}