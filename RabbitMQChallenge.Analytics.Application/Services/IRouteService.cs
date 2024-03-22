using RabbitMQChallenge.Analytics.Application.Models;

namespace RabbitMQChallenge.Analytics.Application.Services
{
    public interface IRouteService
    {
        DeviceRouteVM GetDeviceRoute(string deviceId);
        void UpdateDeviceRoute(string deviceId, DeviceLocationVM deviceLocation);
    }
}
