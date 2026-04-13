using NUnit.Framework;
using googlemapscli.BL.Common;
using googlemapscli.BL.GoogleMaps;
using googlemapscli.BL.GoogleMaps.Models;
using googlemapscli.BL.Tests.Mocks;

namespace googlemapscli.BL.Tests.GoogleMaps;

public class GeolocationCommandTests
{
    private MockGoogleMapsClient _mockClient = null!;
    private GeolocationCommand _command = null!;

    [SetUp]
    public void SetUp()
    {
        _mockClient = new MockGoogleMapsClient();
        _command = new GeolocationCommand(_mockClient);
    }

    [Test]
    public async Task ExecuteAsync_WithValidAddress_ReturnsSuccess()
    {
        var expected = new GeolocationResult("Alexanderplatz, Berlin, Germany", 52.5219, 13.4132);
        _mockClient.SetGeocodeResult(new Result<GeolocationResult>(expected, true, "OK"));

        var result = await _command.ExecuteAsync("Alexanderplatz, Berlin");

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Latitude, Is.EqualTo(52.5219));
        Assert.That(result.Value.Longitude, Is.EqualTo(13.4132));
        Assert.That(_mockClient.LastGeocodeAddress, Is.EqualTo("Alexanderplatz, Berlin"));
    }

    [Test]
    public async Task ExecuteAsync_WithUnknownAddress_ReturnsFailure()
    {
        _mockClient.SetGeocodeResult(new Result<GeolocationResult>(null, false, "No results found."));

        var result = await _command.ExecuteAsync("xyznonexistent12345");

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Does.Contain("No results"));
    }

    [Test]
    public async Task ExecuteAsync_WithEmptyAddress_ReturnsFailure()
    {
        var result = await _command.ExecuteAsync("");

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Does.Contain("address"));
    }

    [Test]
    public async Task ExecuteAsync_WithNullAddress_ReturnsFailure()
    {
        var result = await _command.ExecuteAsync(null!);

        Assert.That(result.IsSuccess, Is.False);
    }
}
