using RabbitMQChallenge.Analytics.Application.Models;

namespace RabbitMQChallenge.Analytics.Application.Services
{
    public class RouteService : IRouteService
    {
        private static readonly List<DeviceRouteVM> _deviceRoutes = [];

        public DeviceRouteVM GetDeviceRoute(string deviceId)
        {
            return _deviceRoutes.Where(g => g.DeviceId == deviceId).FirstOrDefault()!;
        }

        public void UpdateDeviceRoute(string deviceId, DeviceLocationVM deviceLocation)
        {
            bool exists = _deviceRoutes.Exists(c => c.DeviceId == deviceId);
            if (exists)
            {
                _deviceRoutes.First(c => c.DeviceId == deviceId).Routes
                    .Add(new()
                    {
                        Latitude = deviceLocation.Latitude,
                        Longitude = deviceLocation.Longitude,
                        TimeStamp = deviceLocation.TimeStamp,
                    });
            }
            else
            {
                _deviceRoutes.Add(new DeviceRouteVM()
                {
                    DeviceId = deviceId,
                    Routes =
                    [
                        new ()
                        {
                            Latitude = deviceLocation.Latitude,
                            Longitude = deviceLocation.Longitude,
                            TimeStamp = deviceLocation.TimeStamp,
                        }
                    ]
                });
            }
        }
    }
}
