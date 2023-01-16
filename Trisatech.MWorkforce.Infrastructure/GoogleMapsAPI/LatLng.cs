namespace Trisatech.MWorkforce.Infrastructure.GoogleMapsAPI
{
    /// <summary>
    /// This is class consist of two parameter location (latitude, longitude) use for calculate travel time and distance
    /// </summary>
    public class LatLng
    {
        public LatLng(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public override string ToString()
        {
            return $"{Latitude},{Longitude}";
        }
    }
}
