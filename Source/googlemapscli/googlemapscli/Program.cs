using googlemapscli.BL.CommandLineArguments;
using googlemapscli.BL.Logging;

namespace googlemapscli;

public class Program
{
    public static int Main(string[] args)
    {
        var options = CommandLineArgumentsParser.Parse(args);
        if (options is null)
            return 1;

        ILogger logger = new ConsoleLogger();
        logger.Info("googlemapscli started.");

        // Application logic here

        logger.Info("googlemapscli completed.");
        return 0;
    }
}
