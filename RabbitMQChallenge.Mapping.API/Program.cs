using RabbitMQChallenge.Domain.Core.Interfaces;
using RabbitMQChallenge.Infrastructure.IoC;
using RabbitMQChallenge.Mapping.Application.EventHandlers;
using RabbitMQChallenge.Mapping.Application.Events;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DependencyContainer.Register(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

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

IConfiguration config = app.Configuration;

using (var serviceScope = app.Services.CreateScope())
{
    var messageBus = serviceScope.ServiceProvider.GetRequiredService<IMessageBus>();
    messageBus.Subscribe<LocationUpdateEvent, LocationUpdateEventHandler>();
}

app.Run();
