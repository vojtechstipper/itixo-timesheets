using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeEntries.Deleted;
using Itixo.Timesheets.Shared.ConstantObjects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;

public interface IPreDeletedTimeEntryApiService
{
    Task<ApiResult<IEnumerable<TimeEntryGridModel>>> GetPreDeleteTimeEntriesAync(TimeEntriesFilter timeEntriesFilter);
    Task<ApiResult<TimeEntryGridModel>> DeleteTimeEntryPreDeleted(IEnumerable<TimeEntryGridModel> timeEntryPreDeletes);
    Task<ApiResult<TimeEntryGridModel>> DeleteTimeEntry(TimeEntryGridModel filteredTimeEntry);
}

public class PreDeletedTimeEntryApiService : ApiServiceBase, IPreDeletedTimeEntryApiService
{
    public PreDeletedTimeEntryApiService(HttpClient httpClient,
        IDependencies dependencies) : base(httpClient, dependencies) { }



    public async Task<ApiResult<IEnumerable<TimeEntryGridModel>>> GetPreDeleteTimeEntriesAync(TimeEntriesFilter timeEntriesFilter)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.FilteredPreDeleteTimeEntries}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(timeEntriesFilter, Formatting.Indented), Encoding.UTF8, "application/json")
        };

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<IEnumerable<TimeEntryGridModel>>(response);
    }

    public async Task<ApiResult<TimeEntryGridModel>> DeleteTimeEntryPreDeleted(IEnumerable<TimeEntryGridModel> timeEntryPreDeletes)
    {
        var preDeleted = timeEntryPreDeletes.Select(preDelete => new DeleteTimeEntryContract { Id = preDelete.Id }).ToList();
        return await SendRequestDeleteTimeEntryPreDeleted(preDeleted);
    }

    private async Task<ApiResult<TimeEntryGridModel>> SendRequestDeleteTimeEntryPreDeleted(List<DeleteTimeEntryContract> deleted)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response =
            await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.DeleteTimeEntry}", new { TimeEntries = deleted });
        return await responseHandler.HandleApiResponseAsync<TimeEntryGridModel>(response);
    }

    public async Task<ApiResult<TimeEntryGridModel>> DeleteTimeEntry(TimeEntryGridModel filteredTimeEntry)
    {
        var deleted = new List<DeleteTimeEntryContract> { new DeleteTimeEntryContract { Id = filteredTimeEntry.Id } };
        return await SendRequestDeleteTimeEntryPreDeleted(deleted);
    }
}
