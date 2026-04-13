using System.Net.Http.Json;
using System.Text.Json;
using googlemapscli.BL.Common;
using googlemapscli.BL.GoogleMaps.Models;

namespace googlemapscli.BL.GoogleMaps;

public class GoogleMapsClient : IGoogleMapsClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public GoogleMapsClient(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<Result<GeolocationResult>> GeocodeAsync(string address)
    {
        try
        {
            var encodedAddress = Uri.EscapeDataString(address);
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedAddress}&key={_apiKey}";

            var response = await _httpClient.GetFromJsonAsync<GeocodingApiResponse>(url, JsonOptions);

            if (response is null)
                return new Result<GeolocationResult>(null, false, "Empty response from Geocoding API.");

            if (response.Status != "OK")
                return new Result<GeolocationResult>(null, false, $"Geocoding API error: {response.Status}");

            if (response.Results is null || response.Results.Length == 0)
                return new Result<GeolocationResult>(null, false, "No results found for the given address.");

            var first = response.Results[0];
            var location = first.Geometry?.Location;

            if (location is null)
                return new Result<GeolocationResult>(null, false, "No location data in API response.");

            var result = new GeolocationResult(
                first.FormattedAddress ?? address,
                location.Lat,
                location.Lng);

            return new Result<GeolocationResult>(result, true, "OK");
        }
        catch (HttpRequestException ex)
        {
            return new Result<GeolocationResult>(null, false, $"HTTP error: {ex.Message}");
        }
        catch (JsonException ex)
        {
            return new Result<GeolocationResult>(null, false, $"Failed to parse API response: {ex.Message}");
        }
    }

    public async Task<Result<DistanceResult>> GetDistanceAsync(string origin, string destination, string travelMode)
    {
        try
        {
            var encodedOrigin = Uri.EscapeDataString(origin);
            var encodedDest = Uri.EscapeDataString(destination);
            var url = $"https://maps.googleapis.com/maps/api/distancematrix/json" +
                      $"?origins={encodedOrigin}&destinations={encodedDest}" +
                      $"&mode={travelMode}&key={_apiKey}";

            var response = await _httpClient.GetFromJsonAsync<DistanceMatrixApiResponse>(url, JsonOptions);

            if (response is null)
                return new Result<DistanceResult>(null, false, "Empty response from Distance Matrix API.");

            if (response.Status != "OK")
                return new Result<DistanceResult>(null, false, $"Distance Matrix API error: {response.Status}");

            var rows = response.Rows;
            if (rows is null || rows.Length == 0)
                return new Result<DistanceResult>(null, false, "No distance data in API response.");

            var elements = rows[0].Elements;
            if (elements is null || elements.Length == 0)
                return new Result<DistanceResult>(null, false, "No distance data in API response.");

            var element = elements[0];

            if (element.Status != "OK")
                return new Result<DistanceResult>(null, false, $"Route not found: {element.Status}");

            var result = new DistanceResult(
                response.OriginAddresses?.Length > 0 ? response.OriginAddresses[0] : origin,
                response.DestinationAddresses?.Length > 0 ? response.DestinationAddresses[0] : destination,
                travelMode,
                element.Distance?.Text ?? "unknown",
                element.Duration?.Text ?? "unknown");

            return new Result<DistanceResult>(result, true, "OK");
        }
        catch (HttpRequestException ex)
        {
            return new Result<DistanceResult>(null, false, $"HTTP error: {ex.Message}");
        }
        catch (JsonException ex)
        {
            return new Result<DistanceResult>(null, false, $"Failed to parse API response: {ex.Message}");
        }
    }

    // --- API Response DTOs ---

    private class GeocodingApiResponse
    {
        public string? Status { get; set; }
        public GeocodingResult[]? Results { get; set; }
    }

    private class GeocodingResult
    {
        public string? FormattedAddress { get; set; }
        public GeocodingGeometry? Geometry { get; set; }
    }

    private class GeocodingGeometry
    {
        public GeocodingLocation? Location { get; set; }
    }

    private class GeocodingLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    private class DistanceMatrixApiResponse
    {
        public string? Status { get; set; }
        public string[]? OriginAddresses { get; set; }
        public string[]? DestinationAddresses { get; set; }
        public DistanceMatrixRow[]? Rows { get; set; }
    }

    private class DistanceMatrixRow
    {
        public DistanceMatrixElement[]? Elements { get; set; }
    }

    private class DistanceMatrixElement
    {
        public string? Status { get; set; }
        public DistanceMatrixValue? Distance { get; set; }
        public DistanceMatrixValue? Duration { get; set; }
    }

    private class DistanceMatrixValue
    {
        public string? Text { get; set; }
        public int Value { get; set; }
    }
}
