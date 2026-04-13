using googlemapscli.BL.Common;
using googlemapscli.BL.GoogleMaps.Models;

namespace googlemapscli.BL.GoogleMaps;

public class DistanceCommand
{
    private static readonly HashSet<string> ValidTravelModes = new(StringComparer.OrdinalIgnoreCase)
    {
        "driving", "walking", "bicycling", "transit"
    };

    private readonly IGoogleMapsClient _client;

    public DistanceCommand(IGoogleMapsClient client)
    {
        _client = client;
    }

    public async Task<Result<DistanceResult>> ExecuteAsync(string origin, string destination, string travelMode)
    {
        if (string.IsNullOrWhiteSpace(origin))
            return new Result<DistanceResult>(null, false, "No origin provided. Use --from \"address\".");

        if (string.IsNullOrWhiteSpace(destination))
            return new Result<DistanceResult>(null, false, "No destination provided. Use --to \"address\".");

        if (string.IsNullOrWhiteSpace(travelMode))
            return new Result<DistanceResult>(null, false, "No travel mode provided. Use --using \"driving|walking|bicycling|transit\".");

        if (!ValidTravelModes.Contains(travelMode))
            return new Result<DistanceResult>(null, false,
                $"Invalid travel mode '{travelMode}'. Valid options: driving, walking, bicycling, transit.");

        return await _client.GetDistanceAsync(origin, destination, travelMode.ToLowerInvariant());
    }
}
