using Microsoft.Extensions.Configuration;

namespace ApiAutomation.Core.Config;

public static class ConfigurationProvider
{
    public static TestSettings Load()
    {
        var env = Environment.GetEnvironmentVariable("TEST_ENV") ?? "Dev";

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{env}.json", optional: true)
            .AddEnvironmentVariables(prefix: "API_")   // e.g. API_BaseUrl, API_ApiKey
            .Build();

        return config.GetSection("TestSettings").Get<TestSettings>()
               ?? throw new InvalidOperationException("TestSettings section missing.");
    }
}