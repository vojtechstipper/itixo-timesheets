using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.TimeTrackers;
using Itixo.Timesheets.Shared.ConstantObjects;
using Itixo.Timesheets.Shared.Enums;
using Itixo.Timesheets.Shared.Services;
using Itixo.Timesheets.Users.Client.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using AddOrUpdateTimeTrackerAccountResult = Itixo.Timesheets.Contracts.TimeTrackerAccounts.AddOrUpdateTimeTrackerAccountResult;

namespace Itixo.Timesheets.Users.Client.Services;

public interface IAccountApiService
{
    Task<ApiResult<T>> GetAccountAsync<T>(string username);
    Task<ApiResult<AddOrUpdateTimeTrackerAccountResult>> AddOrUpdateAccountAsync(AccountFormModel accountFormModel);
    Task<ApiResult<AccountApiService.TimeTrackerByTypeResponse>> GetByTogglTypeAsync();
}

public class AccountApiService : ApiServiceBase, IAccountApiService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly ICurrentIdentityProvider currentIdentityProvider;

    public AccountApiService(
        HttpClient httpClient,
        IDependencies dependencies,
        ICurrentIdentityProvider currentIdentityProvider) : base(httpClient, dependencies)
    {
        this.currentIdentityProvider = currentIdentityProvider;
        this.httpContextAccessor = dependencies.HttpContextAccessor;
    }

    public async Task<ApiResult<TimeTrackerByTypeResponse>> GetByTogglTypeAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.TimeTrackersByType}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(new { TimeTrackerType = TimeTrackerType.Toggl }, Formatting.Indented), Encoding.UTF8, "application/json")
        };

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);

        return await responseHandler.HandleApiResponseAsync<TimeTrackerByTypeResponse>(response);
    }

    public async Task<ApiResult<T>> GetAccountAsync<T>(string username)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.TimeTrackerAccounts}/{username}");

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);

        return await responseHandler.HandleApiResponseAsync<T>(response);
    }

    public async Task<ApiResult<AddOrUpdateTimeTrackerAccountResult>> AddOrUpdateAccountAsync(AccountFormModel accountFormModel)
    {
        accountFormModel.Email = currentIdentityProvider.Email;

        accountFormModel.Ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);

        var content = new StringContent(
            JsonConvert.SerializeObject(accountFormModel),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        HttpResponseMessage response
            = await httpClient.PostAsync($"{httpClient.BaseAddress}{Endpoints.TimeTrackerAccountsAddOrUpdate}", content);

        return await responseHandler.HandleApiResponseAsync<AddOrUpdateTimeTrackerAccountResult>(response);
    }

    public class TimeTrackerByTypeResponse
    {
        public TimeTrackerContract TimeTracker { get; set; }
    }
}
