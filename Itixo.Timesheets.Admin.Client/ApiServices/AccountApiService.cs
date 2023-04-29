using Itixo.Timesheets.Admin.Client.Models;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Shared.ConstantObjects;
using System.Net.Http;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ApiServices;

public interface IAccountApiService
{
    Task<ApiResult<IdentityInfoModel>> GetCurrentIdentityInfo();
    Task<ApiResult> UpdateCurrentIdentityInfo(IdentityInfoModel identityInfoModel);
}

public class AccountApiService : ApiServiceBase, IAccountApiService
{
    public AccountApiService(HttpClient httpClient, IDependencies dependencies)
        : base(httpClient, dependencies) { }

    public async Task<ApiResult<IdentityInfoModel>> GetCurrentIdentityInfo()
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.GetAsync($"{httpClient.BaseAddress}{Endpoints.GetCurrentIdentityInfo}");
        return await responseHandler.HandleApiResponseAsync<IdentityInfoModel>(response);
    }

    public async Task<ApiResult> UpdateCurrentIdentityInfo(IdentityInfoModel identityInfoModel)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.UpdateCurrentIdentityInfo}", identityInfoModel);
        return await responseHandler.HandleApiResponseAsync<object>(response);
    }
}
