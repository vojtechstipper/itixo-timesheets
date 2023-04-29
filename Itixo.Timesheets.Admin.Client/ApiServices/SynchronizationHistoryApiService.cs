using Itixo.Timesheets.Admin.Client.Models.Configurations;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.SyncHistory;
using Itixo.Timesheets.Shared.ConstantObjects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ApiServices;

public interface ISynchronizationHistoryApiService
{
    Task<ApiResult<List<SyncLogRecordGridItemModel>>> GetSyncLogRecordsAsync(SyncLogRecordsFilter filter);
}

public class SynchronizationHistoryApiService : ApiServiceBase, ISynchronizationHistoryApiService
{
    public SynchronizationHistoryApiService(HttpClient httpClient, IDependencies dependencies)
        : base(httpClient, dependencies) { }

    public async Task<ApiResult<List<SyncLogRecordGridItemModel>>> GetSyncLogRecordsAsync(SyncLogRecordsFilter filter)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);

        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.GetLogRecords}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(filter, Formatting.Indented), Encoding.UTF8, "application/json")
        };

        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<List<SyncLogRecordGridItemModel>>(response);
    }
}
