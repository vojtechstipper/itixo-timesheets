using System.Collections.Generic;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.Workspaces;
using Refit;

namespace TogglSyncShared.ApiInterfaces;

public interface IWorkspaceApi
{
    [Get("/workspaces")]
    Task<List<WorkspaceListContract>> GetAll();
}
