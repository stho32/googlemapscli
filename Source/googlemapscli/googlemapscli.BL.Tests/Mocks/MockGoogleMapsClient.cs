using googlemapscli.BL.Common;
using googlemapscli.BL.GoogleMaps;
using googlemapscli.BL.GoogleMaps.Models;

namespace googlemapscli.BL.Tests.Mocks;

public class MockGoogleMapsClient : IGoogleMapsClient
{
    private Result<GeolocationResult>? _geocodeResult;
    private Result<DistanceResult>? _distanceResult;

    public string? LastGeocodeAddress { get; private set; }
    public string? LastDistanceOrigin { get; private set; }
    public string? LastDistanceDestination { get; private set; }
    public string? LastDistanceTravelMode { get; private set; }

    public void SetGeocodeResult(Result<GeolocationResult> result) => _geocodeResult = result;
    public void SetDistanceResult(Result<DistanceResult> result) => _distanceResult = result;

    public Task<Result<GeolocationResult>> GeocodeAsync(string address)
    {
        LastGeocodeAddress = address;
        var result = _geocodeResult ?? new Result<GeolocationResult>(null, false, "No mock result configured.");
        return Task.FromResult(result);
    }

    public Task<Result<DistanceResult>> GetDistanceAsync(string origin, string destination, string travelMode)
    {
        LastDistanceOrigin = origin;
        LastDistanceDestination = destination;
        LastDistanceTravelMode = travelMode;
        var result = _distanceResult ?? new Result<DistanceResult>(null, false, "No mock result configured.");
        return Task.FromResult(result);
    }
}
