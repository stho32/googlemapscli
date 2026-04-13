using NUnit.Framework;
using googlemapscli.BL.Common;
using googlemapscli.BL.GoogleMaps;
using googlemapscli.BL.GoogleMaps.Models;
using googlemapscli.BL.Tests.Mocks;

namespace googlemapscli.BL.Tests.GoogleMaps;

public class DistanceCommandTests
{
    private MockGoogleMapsClient _mockClient = null!;
    private DistanceCommand _command = null!;

    [SetUp]
    public void SetUp()
    {
        _mockClient = new MockGoogleMapsClient();
        _command = new DistanceCommand(_mockClient);
    }

    [Test]
    public async Task ExecuteAsync_WithValidInputs_ReturnsSuccess()
    {
        var expected = new DistanceResult("Berlin", "Hamburg", "driving", "289 km", "2 hours 52 mins");
        _mockClient.SetDistanceResult(new Result<DistanceResult>(expected, true, "OK"));

        var result = await _command.ExecuteAsync("Berlin", "Hamburg", "driving");

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Distance, Is.EqualTo("289 km"));
        Assert.That(result.Value.Duration, Is.EqualTo("2 hours 52 mins"));
        Assert.That(_mockClient.LastDistanceTravelMode, Is.EqualTo("driving"));
    }

    [Test]
    public async Task ExecuteAsync_WithBicycling_ReturnsSuccess()
    {
        var expected = new DistanceResult("Berlin", "Potsdam", "bicycling", "35 km", "1 hour 45 mins");
        _mockClient.SetDistanceResult(new Result<DistanceResult>(expected, true, "OK"));

        var result = await _command.ExecuteAsync("Berlin", "Potsdam", "bicycling");

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(_mockClient.LastDistanceTravelMode, Is.EqualTo("bicycling"));
    }

    [Test]
    public async Task ExecuteAsync_WithWalking_ReturnsSuccess()
    {
        var expected = new DistanceResult("A", "B", "walking", "5 km", "1 hour");
        _mockClient.SetDistanceResult(new Result<DistanceResult>(expected, true, "OK"));

        var result = await _command.ExecuteAsync("A", "B", "walking");

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(_mockClient.LastDistanceTravelMode, Is.EqualTo("walking"));
    }

    [Test]
    public async Task ExecuteAsync_WithTransit_ReturnsSuccess()
    {
        var expected = new DistanceResult("A", "B", "transit", "10 km", "30 mins");
        _mockClient.SetDistanceResult(new Result<DistanceResult>(expected, true, "OK"));

        var result = await _command.ExecuteAsync("A", "B", "transit");

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(_mockClient.LastDistanceTravelMode, Is.EqualTo("transit"));
    }

    [Test]
    public async Task ExecuteAsync_WithInvalidTravelMode_ReturnsFailure()
    {
        var result = await _command.ExecuteAsync("Berlin", "Hamburg", "helicopter");

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Does.Contain("helicopter"));
    }

    [Test]
    public async Task ExecuteAsync_WithEmptyOrigin_ReturnsFailure()
    {
        var result = await _command.ExecuteAsync("", "Hamburg", "driving");

        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public async Task ExecuteAsync_WithEmptyDestination_ReturnsFailure()
    {
        var result = await _command.ExecuteAsync("Berlin", "", "driving");

        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public async Task ExecuteAsync_WithEmptyTravelMode_ReturnsFailure()
    {
        var result = await _command.ExecuteAsync("Berlin", "Hamburg", "");

        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public async Task ExecuteAsync_WhenApiReturnsError_ReturnsFailure()
    {
        _mockClient.SetDistanceResult(new Result<DistanceResult>(null, false, "API error"));

        var result = await _command.ExecuteAsync("Berlin", "Hamburg", "driving");

        Assert.That(result.IsSuccess, Is.False);
    }
}
