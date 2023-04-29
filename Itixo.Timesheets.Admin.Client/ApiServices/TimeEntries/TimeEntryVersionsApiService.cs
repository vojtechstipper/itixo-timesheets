using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Shared.ConstantObjects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;

public interface ITimeEntryVersionsApiService
{
    Task<ApiResult<TimeEntryVersionsApiService.GetVersionsResult>> GetVersions(int timeEntryId);
}

public class TimeEntryVersionsApiService : ApiServiceBase, ITimeEntryVersionsApiService
{
    public TimeEntryVersionsApiService(HttpClient httpClient, IDependencies dependencies)
        : base(httpClient, dependencies) { }

    public async Task<ApiResult<GetVersionsResult>> GetVersions(int timeEntryId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.TimeEntryVersions}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(new { TimeEntryId = timeEntryId }, Formatting.Indented), Encoding.UTF8, "application/json")
        };

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<GetVersionsResult>(response);
    }

    public class GetVersionsResult
    {
        public List<TimeEntryVersionModel> TimeEntryVersions { get; set; } = new List<TimeEntryVersionModel>();
    }
}
