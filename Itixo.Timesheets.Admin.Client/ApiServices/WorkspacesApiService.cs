using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Contracts.Workspaces;
using Itixo.Timesheets.Shared.ConstantObjects;
using Refit;
using System.Text;
using TogglSyncShared.DataObjects;
using TogglSyncShared.Refit;

namespace Itixo.Timesheets.Admin.Client.ApiServices;

public interface IWorkspacesApiService
{
    Task<IEnumerable<TogglWorkspaceContract>> LoadAccountsWorkspaces(string apiToken);
    Task<ApiResult<bool>> AddWorkspace(AddWorkspaceContract contract);
    Task<ApiResult<bool>> RemoveWorkspace(int togglWorkspaceId);
    Task<ApiResult<IEnumerable<WorkspaceListContract>>> GetWorkspaces();
}

public class WorkspacesApiService : ApiServiceBase, IWorkspacesApiService
{
    public WorkspacesApiService(HttpClient httpClient, IDependencies dependencies)
        : base(httpClient, dependencies) { }

    public async Task<IEnumerable<TogglWorkspaceContract>> LoadAccountsWorkspaces(string apiToken)
    {
        var authHeader = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(apiToken + ":" + "api_token"));

        var refitSettings = new RefitSettings()
        {
            AuthorizationHeaderValueGetter = () => Task.FromResult(authHeader)
        };
        var togglAPI = RestService.For<ITogglAPI>("https://api.track.toggl.com/api/v9", refitSettings);
        List<Workspace> togglWorkspces = await togglAPI.GetWorkspaces();

        var savedWorkspaces = (await GetWorkspaces()).Value.ToList();
        return togglWorkspces.Select(
                togglWorkspace => new TogglWorkspaceContract
                {
                    Exists = savedWorkspaces.Any(x => x.ExternalId == togglWorkspace.Id),
                    ExternalId =
                        togglWorkspace.Id ?? throw new Exception("Toggl workspace missing Id"),
                    WorkspaceId = savedWorkspaces.FirstOrDefault(f => f.ExternalId == togglWorkspace.Id)?.Id ?? 0,
                    WorkspaceName = togglWorkspace.Name
                })
            .ToList();
    }

    public async Task<ApiResult<bool>> AddWorkspace(AddWorkspaceContract contract)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.PostAsJsonAsync($"{httpClient.BaseAddress}{Endpoints.Workspaces}", contract);
        return await responseHandler.HandleApiResponseAsync<bool>(response);
    }

    public async Task<ApiResult<bool>> RemoveWorkspace(int togglWorkspaceId)
    {
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.DeleteAsync($"{httpClient.BaseAddress}{Endpoints.Workspaces}/{togglWorkspaceId}");
        return await responseHandler.HandleApiResponseAsync<bool>(response);
    }

    public async Task<ApiResult<IEnumerable<WorkspaceListContract>>> GetWorkspaces()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{httpClient.BaseAddress}{Endpoints.Workspaces}");
        await authorizer.ConfigureClientToBeAdAuthorized(httpClient);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        return await responseHandler.HandleApiResponseAsync<IEnumerable<WorkspaceListContract>>(response);
    }
}
