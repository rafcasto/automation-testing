using RestSharp;
using Serilog;
using ApiAutomation.Core.Config;

namespace ApiAutomation.Core.Http;

public sealed class ApiClient : IDisposable
{
    private readonly RestClient _client;

    public ApiClient(TestSettings settings)
    {
        var options = new RestClientOptions(settings.BaseUrl)
        {
            Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds),
            ThrowOnAnyError = false
        };
        _client = new RestClient(options);

        if (!string.IsNullOrWhiteSpace(settings.ApiKey))
            _client.AddDefaultHeader("Authorization", $"Bearer {settings.ApiKey}");
    }

    public async Task<RestResponse<T>> GetAsync<T>(string resource, Action<RestRequest>? configure = null)
        => await SendAsync<T>(resource, Method.Get, null, configure);

    public async Task<RestResponse<T>> PostAsync<T>(string resource, object body, Action<RestRequest>? configure = null)
        => await SendAsync<T>(resource, Method.Post, body, configure);

    private async Task<RestResponse<T>> SendAsync<T>(string resource, Method method, object? body, Action<RestRequest>? configure)
    {
        var request = new RestRequest(resource, method);
        if (body is not null) request.AddJsonBody(body);
        configure?.Invoke(request);

        Log.Information("--> {Method} {Resource}", method, resource);
        var response = await _client.ExecuteAsync<T>(request);
        //Log.Information("<-- {Status} ({Ms} ms)", (int)response.StatusCode, response.ResponseTime?.TotalMilliseconds);
        return response;
    }

    public void Dispose() => _client.Dispose();
}