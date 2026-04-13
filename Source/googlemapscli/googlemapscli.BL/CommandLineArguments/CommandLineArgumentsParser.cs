using CommandLine;

namespace googlemapscli.BL.CommandLineArguments;

public static class CommandLineArgumentsParser
{
    public static CommandLineOptions? Parse(string[] args)
    {
        CommandLineOptions? result = null;
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(options => result = options);
        return result;
    }
}
