using Itixo.Timesheets.Admin.Client.Models.AddTimeEntry;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Shared.ConstantObjects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ApiServices;

public interface IAddTimeEntryApiService
{
    Task<ApiResult<bool>> AddTimeEntryAsync(AddTimeEntryFormModel model);
    Task<ApiResult<List<AddTimeEntryFormModel.ProjectComboItem>>> GetProjectsAsync();
    Task<ApiResult<List<AddTimeEntryFormModel.AccountComboItem>>> GetAccountsAsync();
}

public class AddTimeEntryApiService : ApiServiceBase, IAddTimeEntryApiService
{
    public AddTimeEntryApiService(HttpClient httpClient, IDependencies dependencies)
        : base(httpClient, dependencies) { }

    public async Task<ApiResult<bool>> AddTimeEntryAsync(AddTimeEntryFormModel model)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(new Uri($"{httpClient.BaseAddress}{Endpoints.AddTimeEntry}"), model);
        return await responseHandler.HandleApiResponseAsync<bool>(response);
    }

    public async Task<ApiResult<List<AddTimeEntryFormModel.ProjectComboItem>>> GetProjectsAsync()
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.GetAsync(new Uri($"{httpClient.BaseAddress}{Endpoints.ProjectBaseList}"));
        return await responseHandler.HandleApiResponseAsync<List<AddTimeEntryFormModel.ProjectComboItem>>(response);
    }

    public async Task<ApiResult<List<AddTimeEntryFormModel.AccountComboItem>>> GetAccountsAsync()
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.GetAsync(new Uri($"{httpClient.BaseAddress}{Endpoints.TimeTrackerAccounts_OnlyApplicationAccounts}"));
        return await responseHandler.HandleApiResponseAsync<List<AddTimeEntryFormModel.AccountComboItem>>(response);
    }
}
