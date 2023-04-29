using Itixo.Timesheets.Client.Shared.AadAuthorization;
using Microsoft.Identity.Web;

namespace Itixo.Timesheets.Users.Client.Configuration;

public class HttpClientAdAuthorizer : IHttpClientAdAuthorizer
{
    private readonly ITokenAcquisition tokenAcquisition;
    private readonly IConfiguration configuration;

    public HttpClientAdAuthorizer(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
    {
        this.tokenAcquisition = tokenAcquisition;
        this.configuration = configuration;
    }

    public async Task ConfigureClientToBeAdAuthorized(HttpClient httpClient)
    {
        string accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(new[] { configuration["AzureAd:Scope"] });
        httpClient.AddAdAuthorizationHeader(accessToken);
    }
}
