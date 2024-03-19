using RabbitMQChallenge.Mapping.Application.Models;

namespace RabbitMQChallenge.Mapping.Application.Services
{
    public interface IGeoPointService
    {
        IEnumerable<GeoPointVM> GetGeoPoints(string deviceId);
        void AddGeoPoint(GeoPointVM geoPoint);
    }
}
