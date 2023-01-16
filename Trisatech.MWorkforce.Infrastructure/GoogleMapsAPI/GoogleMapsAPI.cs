using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Infrastructure.GoogleMapsAPI
{
    public abstract class GoogleMapsAPI
    {
        protected string MapKey { get; set; }
        public GoogleMapsAPI()
        {
            MapKey = string.Empty;
        }
        public GoogleMapsAPI(string mapKey)
        {
            MapKey = mapKey;
        }
        public abstract Task<int> GetTravelDuration(LatLng source, LatLng destination);
        public abstract Task<double> GetTravelDistance(LatLng source, LatLng destination);
    }
}
