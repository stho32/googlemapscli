using NUnit.Framework;
using googlemapscli.BL.Configuration;

namespace googlemapscli.BL.Tests.Configuration;

public class ApiKeyProviderTests
{
    private const string EnvVarName = "GOOGLE_MAPS_API_KEY";
    private string? _originalValue;

    [SetUp]
    public void SetUp()
    {
        _originalValue = Environment.GetEnvironmentVariable(EnvVarName);
    }

    [TearDown]
    public void TearDown()
    {
        if (_originalValue is not null)
            Environment.SetEnvironmentVariable(EnvVarName, _originalValue);
        else
            Environment.SetEnvironmentVariable(EnvVarName, null);
    }

    [Test]
    public void GetApiKey_WhenSet_ReturnsSuccessWithKey()
    {
        Environment.SetEnvironmentVariable(EnvVarName, "test-api-key-123");

        var result = ApiKeyProvider.GetApiKey();

        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Value, Is.EqualTo("test-api-key-123"));
    }

    [Test]
    public void GetApiKey_WhenNotSet_ReturnsFailure()
    {
        Environment.SetEnvironmentVariable(EnvVarName, null);

        var result = ApiKeyProvider.GetApiKey();

        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Message, Does.Contain("GOOGLE_MAPS_API_KEY"));
    }

    [Test]
    public void GetApiKey_WhenEmpty_ReturnsFailure()
    {
        Environment.SetEnvironmentVariable(EnvVarName, "");

        var result = ApiKeyProvider.GetApiKey();

        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public void GetApiKey_WhenWhitespace_ReturnsFailure()
    {
        Environment.SetEnvironmentVariable(EnvVarName, "   ");

        var result = ApiKeyProvider.GetApiKey();

        Assert.That(result.IsSuccess, Is.False);
    }
}
