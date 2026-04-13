using googlemapscli.BL.CommandLineArguments;
using googlemapscli.BL.Configuration;
using googlemapscli.BL.GoogleMaps;
using googlemapscli.BL.Logging;

namespace googlemapscli;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var options = CommandLineArgumentsParser.Parse(args);
        if (options is null)
            return 1;

        ILogger logger = new ConsoleLogger();

        if (!options.Geolocation && !options.Distance)
        {
            logger.Error("No command specified. Use --geolocation or --distance.");
            return 1;
        }

        var apiKeyResult = ApiKeyProvider.GetApiKey();
        if (!apiKeyResult.IsSuccess)
        {
            logger.Error(apiKeyResult.Message);
            return 1;
        }

        using var httpClient = new HttpClient();
        var client = new GoogleMapsClient(httpClient, apiKeyResult.Value!);

        if (options.Geolocation)
            return await HandleGeolocation(client, options, logger);

        if (options.Distance)
            return await HandleDistance(client, options, logger);

        return 0;
    }

    private static async Task<int> HandleGeolocation(IGoogleMapsClient client, CommandLineOptions options, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(options.Of))
        {
            logger.Error("No address provided. Use --of \"address\".");
            return 1;
        }

        var command = new GeolocationCommand(client);
        var result = await command.ExecuteAsync(options.Of);

        if (!result.IsSuccess)
        {
            logger.Error(result.Message);
            return 1;
        }

        var geo = result.Value!;
        Console.WriteLine($"Adresse: {geo.FormattedAddress}");
        Console.WriteLine($"Breitengrad: {geo.Latitude}");
        Console.WriteLine($"Laengengrad: {geo.Longitude}");
        return 0;
    }

    private static async Task<int> HandleDistance(IGoogleMapsClient client, CommandLineOptions options, ILogger logger)
    {
        if (string.IsNullOrWhiteSpace(options.From))
        {
            logger.Error("No origin provided. Use --from \"address\".");
            return 1;
        }

        if (string.IsNullOrWhiteSpace(options.To))
        {
            logger.Error("No destination provided. Use --to \"address\".");
            return 1;
        }

        if (string.IsNullOrWhiteSpace(options.Using))
        {
            logger.Error("No travel mode provided. Use --using \"driving|walking|bicycling|transit\".");
            return 1;
        }

        var command = new DistanceCommand(client);
        var result = await command.ExecuteAsync(options.From, options.To, options.Using);

        if (!result.IsSuccess)
        {
            logger.Error(result.Message);
            return 1;
        }

        var dist = result.Value!;
        Console.WriteLine($"Von: {dist.Origin}");
        Console.WriteLine($"Nach: {dist.Destination}");
        Console.WriteLine($"Verkehrsmittel: {dist.TravelMode}");
        Console.WriteLine($"Distanz: {dist.Distance}");
        Console.WriteLine($"Dauer: {dist.Duration}");
        return 0;
    }
}
