using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Infrastructure.GoogleMapsAPI
{
    public interface IGoogleMapsAPIService
    {
        Task<double> GetTravelDistance(LatLng source, LatLng destination);
        Task<int> GetTravelDuration(LatLng source, LatLng destination);
    }
}
