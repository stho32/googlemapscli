using googlemapscli.BL.Common;

namespace googlemapscli.BL.Configuration;

public static class ApiKeyProvider
{
    private const string EnvVarName = "GOOGLE_MAPS_API_KEY";

    public static Result<string> GetApiKey()
    {
        var key = Environment.GetEnvironmentVariable(EnvVarName);

        if (string.IsNullOrWhiteSpace(key))
            return new Result<string>(null, false,
                $"Environment variable '{EnvVarName}' is not set. " +
                $"Set it with: export {EnvVarName}=your-api-key");

        return new Result<string>(key, true, "API key loaded.");
    }
}
