using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Shared.ConstantObjects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;

public interface ITimeEntryStateChangesApiService
{
    Task<ApiResult<TimeEntryStateChangesApiService.GetStateChangesResponse>> GetStateChangesAsync(int timeEntryId);
}

public class TimeEntryStateChangesApiService : ApiServiceBase, ITimeEntryStateChangesApiService
{
    public TimeEntryStateChangesApiService(HttpClient httpClient, IDependencies dependencies)
        : base(httpClient, dependencies)
    {
    }

    public async Task<ApiResult<GetStateChangesResponse>> GetStateChangesAsync(int timeEntryId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.TimeEntryStateChanges}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(new { TimeEntryId = timeEntryId }, Formatting.Indented), Encoding.UTF8, "application/json")
        };

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<GetStateChangesResponse>(response);
    }

    public class GetStateChangesResponse
    {
        public List<TimeEntryStateChangeModel> TimeEntryStateChanges { get; set; }
        = new List<TimeEntryStateChangeModel>();
    }
}
