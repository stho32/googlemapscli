using NUnit.Framework;
using googlemapscli.BL.CommandLineArguments;

namespace googlemapscli.BL.Tests.CommandLineArguments;

public class CommandLineOptionsTests
{
    [Test]
    public void Parse_GeolocationWithOf_ReturnsCorrectOptions()
    {
        var args = new[] { "--geolocation", "--of", "Alexanderplatz, Berlin" };

        var result = CommandLineArgumentsParser.Parse(args);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Geolocation, Is.True);
        Assert.That(result.Of, Is.EqualTo("Alexanderplatz, Berlin"));
    }

    [Test]
    public void Parse_DistanceWithAllOptions_ReturnsCorrectOptions()
    {
        var args = new[] { "--distance", "--from", "Berlin", "--to", "Hamburg", "--using", "driving" };

        var result = CommandLineArgumentsParser.Parse(args);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Distance, Is.True);
        Assert.That(result.From, Is.EqualTo("Berlin"));
        Assert.That(result.To, Is.EqualTo("Hamburg"));
        Assert.That(result.Using, Is.EqualTo("driving"));
    }

    [Test]
    public void Parse_DistanceWithBicycling_ReturnsCorrectOptions()
    {
        var args = new[] { "--distance", "--from", "A", "--to", "B", "--using", "bicycling" };

        var result = CommandLineArgumentsParser.Parse(args);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Using, Is.EqualTo("bicycling"));
    }

    [Test]
    public void Parse_VerboseFlag_ReturnsVerboseTrue()
    {
        var args = new[] { "--geolocation", "--of", "Berlin", "-v" };

        var result = CommandLineArgumentsParser.Parse(args);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Verbose, Is.True);
    }

    [Test]
    public void Parse_NoArgs_ReturnsOptions()
    {
        var args = Array.Empty<string>();

        var result = CommandLineArgumentsParser.Parse(args);

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Geolocation, Is.False);
        Assert.That(result.Distance, Is.False);
    }
}
