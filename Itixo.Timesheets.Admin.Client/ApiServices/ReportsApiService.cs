using Itixo.Timesheets.Admin.Client.Models.Reports;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.Clients;
using Itixo.Timesheets.Contracts.Invoices;
using Itixo.Timesheets.Contracts.Projects;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Shared.ConstantObjects;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ApiServices;

public interface IReportsApiService
{
    Task<ApiResult<IEnumerable<ProjectBaseContract>>> GetProjectsAsync(IEnumerable<int> clientIds);
    Task<ApiResult<IEnumerable<AccountReportGridItemModel>>> GetReportUserGridItemsAsync(ReportsQueryFilter filter);
    Task<ApiResult<AccountReportGridSummaryModel>> GetReportUserGridSummaryAsync(ReportsQueryFilter filter);
    Task<ApiResult<IEnumerable<ClientListContract>>> GetClientsAsync();
    Task<ApiResult<InvoiceAssignmentResult>> AssignInvoiceAsync(SummaryTimeEntriesInvoiceAssignmentParameter parameter);
}

public class ReportsApiService : ApiServiceBase, IReportsApiService
{
    public ReportsApiService(HttpClient httpClient, IDependencies dependencies)
        : base(httpClient, dependencies) { }

    public async Task<ApiResult<IEnumerable<ProjectBaseContract>>> GetProjectsAsync(IEnumerable<int> clientIds)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.Projects}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(clientIds, Formatting.Indented), Encoding.UTF8, "application/json")
        };

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<IEnumerable<ProjectBaseContract>>(response);
    }

    public async Task<ApiResult<IEnumerable<AccountReportGridItemModel>>> GetReportUserGridItemsAsync(ReportsQueryFilter filter)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.Reports}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(filter, Formatting.Indented), Encoding.UTF8, "application/json")
        };

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<IEnumerable<AccountReportGridItemModel>>(response);
    }
    public async Task<ApiResult<AccountReportGridSummaryModel>> GetReportUserGridSummaryAsync(ReportsQueryFilter filter)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.ReportsSummary}")
        {
            Content = new StringContent(JsonConvert.SerializeObject(filter, Formatting.Indented), Encoding.UTF8, "application/json")
        };

        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<AccountReportGridSummaryModel>(response);
    }

    public async Task<ApiResult<IEnumerable<ClientListContract>>> GetClientsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.Clients}");
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<IEnumerable<ClientListContract>>(response);
    }

    public async Task<ApiResult<InvoiceAssignmentResult>> AssignInvoiceAsync(SummaryTimeEntriesInvoiceAssignmentParameter parameter)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.AssignInvoiceToSummaries}", parameter);
        return await responseHandler.HandleApiResponseAsync<InvoiceAssignmentResult>(response);
    }
}
