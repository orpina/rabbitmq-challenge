using RabbitMQChallenge.Domain.Core.Bus;
using RabbitMQChallenge.Infrastructure.IoC;
using RabbitMQChallenge.Mapping.Domain.BusHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DependencyContainer.Register(builder.Services);

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

IConfiguration config = app.Configuration;

using (var serviceScope = app.Services.CreateScope())
{
    var eventBus = serviceScope.ServiceProvider.GetRequiredService<IBus>();
    eventBus.Subscribe<GeoUpdateHandler>(config["BusConfig:GeoUpdateQueue"]!);
}

app.Run();
