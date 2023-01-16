using Trisatech.MWorkforce.Infrastructure.GoogleMapsAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Trisatech.MWorkforce.Infrastructure.Services
{
    public class GoogleMapsService : IGoogleMapsAPIService
    {
        public Task<double> GetTravelDistance(LatLng source, LatLng destination)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTravelDuration(LatLng source, LatLng destination)
        {
            throw new NotImplementedException();
        }
    }
}
