namespace googlemapscli.BL.GoogleMaps.Models;

public record DistanceResult(
    string Origin,
    string Destination,
    string TravelMode,
    string Distance,
    string Duration);
