using RabbitMQChallenge.Mapping.Application.Models;

namespace RabbitMQChallenge.Mapping.Application.Services
{
    public class GeoPointService : IGeoPointService
    {
        private static readonly List<GeoPointVM> _geoPoints = [];

        public IEnumerable<GeoPointVM> GetGeoPoints(string deviceId) 
        {
            return _geoPoints
                .Where(g => g.DeviceId == deviceId)
                .OrderByDescending(g => g.TimeStamp);
        }

        public void AddGeoPoint(GeoPointVM geoPoint)
        {
            _geoPoints.Add(geoPoint);
        }
    }
}
