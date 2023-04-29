using Itixo.Timesheets.Shared.Converters;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TogglSyncShared;

public interface IApiConnectorFactory
{
    T CreateApiConnector<T>();
}

public class ApiConnectorFactory : IApiConnectorFactory
{
    private readonly IConfiguration configuration;

    public ApiConnectorFactory(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public T CreateApiConnector<T>()
    {
        var authenticatedHttpClientHandler = new AuthenticatedHttpClientHandler(
            async () =>
            {
                IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(configuration["AzureAd:ClientId"])
                    .WithClientSecret(configuration["AzureAd:ClientSecret"])
                    .WithTenantId(configuration["AzureAd:TenantId"])
                    .Build();

                AuthenticationResult authenticationResult = await app.AcquireTokenForClient(new[] { configuration["TogglSync:AzureAd:Scope"] }).ExecuteAsync();
                return authenticationResult.AccessToken;
               });

        var jsonSerializerSettings = new JsonSerializerSettings { Converters = new List<JsonConverter> { new CetDateTimeOffsetConverter() } };
        return RestService.For<T>(new HttpClient(authenticatedHttpClientHandler) { BaseAddress = new Uri(configuration["TimesheetsApiUri"]) },
            new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(jsonSerializerSettings)
            });
    }
}

public class AuthenticatedHttpClientHandler : HttpClientHandler
{
    private readonly Func<Task<string>> getToken;

    public AuthenticatedHttpClientHandler(Func<Task<string>> getToken)
    {
        this.getToken = getToken;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AuthenticationHeaderValue auth = request.Headers.Authorization;

        if (auth == null)
        {
            string token = await getToken().ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var message = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        return message;
    }
}
