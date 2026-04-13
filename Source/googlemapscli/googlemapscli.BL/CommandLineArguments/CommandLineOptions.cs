using CommandLine;

namespace googlemapscli.BL.CommandLineArguments;

public class CommandLineOptions
{
    [Option('v', "verbose", Required = false, HelpText = "Enable verbose output.")]
    public bool Verbose { get; set; }
}
