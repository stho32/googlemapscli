using CommandLine;

namespace googlemapscli.BL.CommandLineArguments;

public class CommandLineOptions
{
    [Option('v', "verbose", Required = false, HelpText = "Enable verbose output.")]
    public bool Verbose { get; set; }

    [Option("geolocation", Required = false, HelpText = "Get geolocation coordinates for an address.")]
    public bool Geolocation { get; set; }

    [Option("of", Required = false, HelpText = "Address to geocode (used with --geolocation).")]
    public string? Of { get; set; }

    [Option("distance", Required = false, HelpText = "Calculate distance and duration between two locations.")]
    public bool Distance { get; set; }

    [Option("from", Required = false, HelpText = "Origin address (used with --distance).")]
    public string? From { get; set; }

    [Option("to", Required = false, HelpText = "Destination address (used with --distance).")]
    public string? To { get; set; }

    [Option("using", Required = false, HelpText = "Travel mode: driving, walking, bicycling, transit (used with --distance).")]
    public string? Using { get; set; }
}
