using Itixo.Timesheets.Admin.Client.Models.TimeTrackerAccounts;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Contracts.TimeTrackers;
using Itixo.Timesheets.Shared.ConstantObjects;
using Itixo.Timesheets.Shared.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ApiServices;

public interface ITimeTrackerAccountsApiService
{
    Task<ApiResult<IEnumerable<T>>> GetAccountsAsync<T>();
    Task<ApiResult<AddOrUpdateTimeTrackerAccountResult>> AddOrUpdateAccountAsync(
        TimeTrackerAccountDetailModel timeTrackerAccount);
    Task<ApiResult<bool>> DeleteAccountAsync(int id);
    Task<ApiResult<TimeTrackerAccountsApiService.TimeTrackersResponse>> GetTimeEntryTrackersAsync();
}

public class TimeTrackerAccountsApiService : ApiServiceBase, ITimeTrackerAccountsApiService
{
    private readonly ICurrentIdentityProvider currentIdentityProvider;

    public TimeTrackerAccountsApiService(HttpClient httpClient, IDependencies dependencies, ICurrentIdentityProvider currentIdentityProvider)
        : base(httpClient, dependencies)
    {
        this.currentIdentityProvider = currentIdentityProvider;
    }

    public async Task<ApiResult<IEnumerable<T>>> GetAccountsAsync<T>()
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.TimeTrackerAccounts}");
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<IEnumerable<T>>(response);
    }

    public async Task<ApiResult<AddOrUpdateTimeTrackerAccountResult>> AddOrUpdateAccountAsync(TimeTrackerAccountDetailModel timeTrackerAccount)
    {
        timeTrackerAccount.Email = currentIdentityProvider.Email;

        HttpResponseMessage response;

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        if (timeTrackerAccount.Id > 0)
        {
            response = await httpClient.PutAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.TimeTrackerAccounts}/{timeTrackerAccount.Id}", timeTrackerAccount);
        }
        else
        {
            response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.TimeTrackerAccounts}", timeTrackerAccount);
        }

        return await responseHandler.HandleApiResponseAsync<AddOrUpdateTimeTrackerAccountResult>(response);
    }

    public async Task<ApiResult<bool>> DeleteAccountAsync(int id)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.DeleteAsync($"{httpClient.BaseAddress}{Endpoints.TimeTrackerAccounts}/{id}");
        return await responseHandler.HandleApiResponseAsync<bool>(response);
    }

    public async Task<ApiResult<TimeTrackersResponse>> GetTimeEntryTrackersAsync()
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.TimeTrackers}");
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<TimeTrackersResponse>(response);
    }

    public class TimeTrackersResponse
    {
        public List<TimeTrackerContract> TimeTrackers { get; set; }
    }
}
