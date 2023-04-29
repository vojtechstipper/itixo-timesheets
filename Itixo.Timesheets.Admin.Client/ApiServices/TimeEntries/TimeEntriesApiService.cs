using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.Clients;
using Itixo.Timesheets.Contracts.Invoices;
using Itixo.Timesheets.Contracts.Projects;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Contracts.TimeEntries.Bans;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Shared.ConstantObjects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;

public interface ITimeEntriesApiService
{
    Task<ApiResult<bool>> ApproveTimeEntryDraft(TimeEntryGridModel itemContractItemContract);
    Task<ApiResult<bool>> ApproveTimeEntryDrafts(IEnumerable<TimeEntryGridModel> items);
    Task<ApiResult<TimeEntryGridModel>> BanTimeEntryDraft(TimeEntryGridModel filteredTimeEntry);
    Task<ApiResult<TimeEntryGridModel>> BanTimeEntryDrafts(IEnumerable<TimeEntryGridModel> timeEntryDrafts);
    Task<ApiResult<IEnumerable<TimeEntryGridModel>>> GetFilteredTimeEntriesAync(TimeEntriesFilter timeEntriesFilter);
    Task<ApiResult<IEnumerable<ProjectBaseContract>>> GetProjectsAsync(List<int> clientIds);
    Task<ApiResult<IEnumerable<AccountListContract>>> GetUsersAsync();
    Task<ApiResult<IEnumerable<ClientListContract>>> GetClientsAsync();
    Task<ApiResult<TimeEntriesGridPageInfoContract>> GetFilteredTimeEntriesPageCountAsync(TimeEntriesFilter timeEntriesFilter);
    Task<ApiResult<InvoiceAssignmentResult>> AssignInvoiceAsync(TimeEntriesInvoiceAssignmentParameter parameter);
}

public class TimeEntriesApiService : ApiServiceBase, ITimeEntriesApiService
{
    public TimeEntriesApiService(HttpClient httpClient, IDependencies dependencies)
        : base(httpClient, dependencies) { }

    public async Task<ApiResult<IEnumerable<ProjectBaseContract>>> GetProjectsAsync(List<int> clientIds)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.Projects}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(clientIds, Formatting.Indented), Encoding.UTF8, "application/json")
        };

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<IEnumerable<ProjectBaseContract>>(response);
    }

    public async Task<ApiResult<IEnumerable<AccountListContract>>> GetUsersAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.TimeTrackerAccounts}");
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<IEnumerable<AccountListContract>>(response);
    }

    public async Task<ApiResult<IEnumerable<ClientListContract>>> GetClientsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.Clients}");
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<IEnumerable<ClientListContract>>(response);
    }

    public async Task<ApiResult<TimeEntriesGridPageInfoContract>> GetFilteredTimeEntriesPageCountAsync(TimeEntriesFilter timeEntriesFilter)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.FilteredTimeEntriesPagingInfo}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(timeEntriesFilter, Formatting.Indented), Encoding.UTF8, "application/json")
        };

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<TimeEntriesGridPageInfoContract>(response);
    }

    public async Task<ApiResult<InvoiceAssignmentResult>> AssignInvoiceAsync(TimeEntriesInvoiceAssignmentParameter parameter)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.AssignInvoiceToTimeEntries}", parameter);
        return await responseHandler.HandleApiResponseAsync<InvoiceAssignmentResult>(response);
    }

    public async Task<ApiResult<bool>> ApproveTimeEntryDraft(TimeEntryGridModel itemContractItemContract)
    {
        var draft = new ApproveTimeEntryDraftContract { TimeEntryDraftId = itemContractItemContract.Id };
        var drafts = new List<ApproveTimeEntryDraftContract>(new[] { draft });
        return await SendRequestApproveTimeEntryDrafts(drafts);
    }

    public async Task<ApiResult<bool>> ApproveTimeEntryDrafts(IEnumerable<TimeEntryGridModel> items)
    {
        var drafts = items.Select(item => new ApproveTimeEntryDraftContract { TimeEntryDraftId = item.Id }).ToList();
        return await SendRequestApproveTimeEntryDrafts(drafts);
    }

    private async Task<ApiResult<bool>> SendRequestApproveTimeEntryDrafts(IEnumerable<ApproveTimeEntryDraftContract> drafts)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response =
            await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.ApproveTimeEntry}", new { TimeEntries = drafts });
        return await responseHandler.HandleApiResponseAsync<bool>(response);
    }

    public async Task<ApiResult<TimeEntryGridModel>> BanTimeEntryDraft(TimeEntryGridModel filteredTimeEntry)
    {
        var bans = new List<BanTimeEntryDraftContract> { new BanTimeEntryDraftContract { Id = filteredTimeEntry.Id } };
        return await SendRequestBanTimeEntryDrafts(bans);
    }

    public async Task<ApiResult<TimeEntryGridModel>> BanTimeEntryDrafts(IEnumerable<TimeEntryGridModel> timeEntryDrafts)
    {
        var bans = timeEntryDrafts.Select(draft => new BanTimeEntryDraftContract { Id = draft.Id }).ToList();
        return await SendRequestBanTimeEntryDrafts(bans);
    }

    private async Task<ApiResult<TimeEntryGridModel>> SendRequestBanTimeEntryDrafts(List<BanTimeEntryDraftContract> bans)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response =
            await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.BanTimeEntry}", new { TimeEntries = bans });
        return await responseHandler.HandleApiResponseAsync<TimeEntryGridModel>(response);
    }

    public async Task<ApiResult<IEnumerable<TimeEntryGridModel>>> GetFilteredTimeEntriesAync(TimeEntriesFilter timeEntriesFilter)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.FilteredTimeEntries}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(timeEntriesFilter, Formatting.Indented), Encoding.UTF8, "application/json")
        };

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<IEnumerable<TimeEntryGridModel>>(response);
    }
}
