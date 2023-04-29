using Itixo.Timesheets.Client.Shared.AadAuthorization;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Shared.ConstantObjects;
using Itixo.Timesheets.Shared.Messaging;
using Microsoft.Identity.Web;

namespace Itixo.Timesheets.Admin.Client.ApiServices;

public interface ISynchronizationApiService
{
    Task RunSynchronizationAsync(TriggerSyncMessage triggerSyncMessage);
}

public class SynchronizationApiService : ApiServiceBase, ISynchronizationApiService
{
    private readonly IDependencies dependencies;
    private readonly ITokenAcquisition tokenAcquisition;

    public SynchronizationApiService(HttpClient httpClient, IDependencies dependencies, ITokenAcquisition tokenAcquisition) : base(httpClient, dependencies)
    {
        this.dependencies = dependencies;
        this.tokenAcquisition = tokenAcquisition;
    }

    public async Task RunSynchronizationAsync(TriggerSyncMessage triggerSyncMessage)
    {
        string accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(new[] { configuration["AzureAd:Scope"] });
        httpClient.AddAdAuthorizationHeader(accessToken);
        await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.RunSynchronization}", triggerSyncMessage);
    }
}
