using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using TogglSyncShared.DataObjects;

namespace TogglSyncShared.Refit;

[Headers("Authorization: Basic")]
public interface ITogglAPI
{
    [Get("/me/time_entries")]
    Task<List<TogglTimeEntry>> GetTimeEntries(object parameters);

    [Get("/workspaces/{workspaceId}/clients/{clientId}")]
    Task<Client> GetClient(int workspaceId, int clientId);

    [Get("/workspaces")]
    Task<List<Workspace>> GetWorkspaces();

    [Get("/me/projects")]
    Task<List<TogglProject>> GetProjects();
}
