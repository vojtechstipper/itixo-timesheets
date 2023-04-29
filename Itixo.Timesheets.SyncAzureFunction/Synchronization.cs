using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Itixo.Timesheets.AF.Sync;

public class Synchronization
{
    private readonly ILogger _logger;
    private const string SyncApiUrlKey = "SyncApiUrl";
    private const string SyncEndpoint = "synchronization/run";
    private const string ClientId = "ClientId";
    private const string Scope = "Scope";
    private const string ClientSecret = "ClientSecret";
    private const string Tenant = "Tenant";

    public Synchronization(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<Synchronization>();
    }

    [Function("Synchronization")]
    public async Task Run([TimerTrigger("%SyncInterval%")] TimerInfo myTimer)
    {
        _logger.LogInformation($"Start sync at: {DateTime.Now}");
        await RunSync();
    }

    private static async Task RunSync()
    {
        IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(Environment.GetEnvironmentVariable(ClientId))
                      .WithTenantId(Environment.GetEnvironmentVariable(Tenant))
                      .WithClientSecret(Environment.GetEnvironmentVariable(ClientSecret))
                      .Build();

        AuthenticationResult authenticationResult = await app.AcquireTokenForClient(new[] { Environment.GetEnvironmentVariable(Scope) }).ExecuteAsync();

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
            await client.PostAsJsonAsync($"{Environment.GetEnvironmentVariable(SyncApiUrlKey)}{SyncEndpoint}", new { });
        }
    }
}

public class TimerInfo
{
}
