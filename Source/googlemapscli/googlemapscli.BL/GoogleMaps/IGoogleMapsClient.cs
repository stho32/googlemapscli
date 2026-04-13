using googlemapscli.BL.Common;
using googlemapscli.BL.GoogleMaps.Models;

namespace googlemapscli.BL.GoogleMaps;

public interface IGoogleMapsClient
{
    Task<Result<GeolocationResult>> GeocodeAsync(string address);
    Task<Result<DistanceResult>> GetDistanceAsync(string origin, string destination, string travelMode);
}
