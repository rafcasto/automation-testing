namespace ApiAutomation.Core.Config;

public sealed class TestSettings
{
    public string BaseUrl { get; init; } = string.Empty;
    public int TimeoutSeconds { get; init; } = 30;
    public string? ApiKey { get; init; }
}