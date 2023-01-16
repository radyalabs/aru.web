using Newtonsoft.Json;

namespace Trisatech.MWorkforce.Infrastructure.GoogleMapsAPI
{
    public class GoogleMapsAPIService : GoogleMapsAPI
    {
        private readonly string _format;
        private readonly string _googleMapsApiUrl;
        private const string API_OK_RESPONSE = "OK";
        public GoogleMapsAPIService(string baseUrl, string mapKey, string format):base(mapKey)
        {
            _format = format;
            _googleMapsApiUrl = baseUrl ?? "https://maps.googleapis.com/maps/api";
        }

        public override async Task<double> GetTravelDistance(LatLng source, LatLng destination)
        {
            double result = 0;

            using (var client = new HttpClient())
            {
                Uri uri = new Uri($"{_googleMapsApiUrl}/distancematrix/{_format}?origins={destination.ToString()}&destinations={source.ToString()}&mode=driving&language=en-US&key={MapKey}");

                try
                {
                    var response = await client.GetAsync(uri);

                    if (!response.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine(response.StatusCode.ToString());
                        System.Diagnostics.Debug.WriteLine(response.ToString());
                    }
                    else
                    {
                        var byteResult = await response.Content.ReadAsByteArrayAsync();
                        var strResult = Encoding.UTF8.GetString(byteResult, 0, byteResult.Length);

                        var mapResponse = JsonConvert.DeserializeObject<GoogleMapsAPIResponse>(strResult);
                        if(mapResponse !=  null && mapResponse.Status.Equals(API_OK_RESPONSE) && mapResponse.Rows.Length > 0)
                        {
                            if(mapResponse.Rows[0].Elements.Length>0)
                                result = mapResponse.Rows[0].Elements[0].Distance.Value;
                        }
                        else if(mapResponse!=null)
                        {
                            throw new Exception($"{mapResponse.Status}:{mapResponse.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }

        public override async Task<int> GetTravelDuration(LatLng source, LatLng destination)
        {
            int result = 0;
            using (var client = new HttpClient())
            {
                Uri uri = new Uri($"{_googleMapsApiUrl}/distancematrix/{_format}?origins={destination.ToString()}&destinations={source.ToString()}&mode=driving&language=en-US&key={MapKey}");

                try
                {
                    var response = await client.GetAsync(uri);

                    if (!response.IsSuccessStatusCode)
                    {
                        System.Diagnostics.Debug.WriteLine(response.StatusCode.ToString());
                        System.Diagnostics.Debug.WriteLine(response.ToString());
                    }
                    else
                    {
                        var byteResult = await response.Content.ReadAsByteArrayAsync();
                        var strResult = Encoding.UTF8.GetString(byteResult, 0, byteResult.Length);

                        var mapResponse = JsonConvert.DeserializeObject<GoogleMapsAPIResponse>(strResult);
                        if (mapResponse != null && mapResponse.Status.Equals("OK") && mapResponse.Rows.Length > 0)
                        {
                            if (mapResponse.Rows[0].Elements.Length > 0)
                                result = (int)mapResponse.Rows[0].Elements[0].Duration.Value;
                        }
                        else if (mapResponse != null)
                        {
                            throw new Exception($"{mapResponse.Status}:{mapResponse.ErrorMessage}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return result;
        }
    }
}

