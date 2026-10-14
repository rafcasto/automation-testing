using ApiAutomation.Core.Config;
using ApiAutomation.Core.Http;
using NUnit.Framework;
using Serilog;

namespace ApiAutomation.Tests.Base;

public abstract class ApiTestBase
{
    protected ApiClient Client { get; private set; } = null!;
    protected TestSettings Settings { get; private set; } = null!;

    [OneTimeSetUp]
    public void GlobalSetup()
    {
        Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        Settings = ConfigurationProvider.Load();
        Client = new ApiClient(Settings);
    }

    [OneTimeTearDown]
    public void GlobalTeardown()
    {
        Client.Dispose();
        Log.CloseAndFlush();
    }
}