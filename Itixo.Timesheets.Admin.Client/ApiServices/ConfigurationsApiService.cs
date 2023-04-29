using Itixo.Timesheets.Admin.Client.Models.Configurations;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.Configurations;
using Itixo.Timesheets.Shared.ConstantObjects;
using System.Net.Http;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ApiServices;

public interface IConfigurationsApiService
{
    Task<ApiResult<SyncDateRangeModel>> AddOrUpdateSyncBusinessDaysAsync(SyncDateRangeModel dateRangeModel);
    Task<ApiResult<SyncDateRangeModel>> GetSyncBusinessDaysAsync();
    Task<ApiResult<SyncSharedLockContract>> IsSyncLockedAsync();
}

public class ConfigurationsApiService : ApiServiceBase, IConfigurationsApiService
{
    public ConfigurationsApiService(HttpClient httpClient, IDependencies dependencies)
        : base(httpClient, dependencies) { }

    public async Task<ApiResult<SyncDateRangeModel>> AddOrUpdateSyncBusinessDaysAsync(SyncDateRangeModel dateRangeModel)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);

        HttpResponseMessage response;
        if (dateRangeModel.StartSyncBusinessDaysAgoId == 0 && dateRangeModel.StopSyncBusinessDaysAgoId == 0)
        {
            response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.AddSyncDateRange}", dateRangeModel);
        }
        else
        {
            response = await httpClient.PutAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.UpdateSyncDateRange}", dateRangeModel);
        }

        return await responseHandler.HandleApiResponseAsync<SyncDateRangeModel>(response);
    }

    public async Task<ApiResult<SyncDateRangeModel>> GetSyncBusinessDaysAsync()
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.GetAsync($"{httpClient.BaseAddress}{Endpoints.GetSyncDateRange}");
        return await responseHandler.HandleApiResponseAsync<SyncDateRangeModel>(response);
    }

    public async Task<ApiResult<SyncSharedLockContract>> IsSyncLockedAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.GetSyncLock}");

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);

        return await responseHandler.HandleApiResponseAsync<SyncSharedLockContract>(response);
    }
}
