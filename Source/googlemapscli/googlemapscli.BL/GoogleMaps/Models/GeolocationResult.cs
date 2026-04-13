namespace googlemapscli.BL.GoogleMaps.Models;

public record GeolocationResult(
    string FormattedAddress,
    double Latitude,
    double Longitude);
