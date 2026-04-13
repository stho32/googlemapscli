using googlemapscli.BL.Common;
using googlemapscli.BL.GoogleMaps.Models;

namespace googlemapscli.BL.GoogleMaps;

public class GeolocationCommand
{
    private readonly IGoogleMapsClient _client;

    public GeolocationCommand(IGoogleMapsClient client)
    {
        _client = client;
    }

    public async Task<Result<GeolocationResult>> ExecuteAsync(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return new Result<GeolocationResult>(null, false, "No address provided. Use --of \"address\".");

        return await _client.GeocodeAsync(address);
    }
}
