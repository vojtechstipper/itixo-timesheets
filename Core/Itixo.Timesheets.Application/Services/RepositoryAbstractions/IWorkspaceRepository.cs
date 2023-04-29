using System.Collections.Generic;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.Workspaces;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface IWorkspaceRepository : IEntityRepository<Workspace, int>
{
    Task<string> RemoveWorkspaceIdAsync(int togglWorkspaceId);
    Task<string> AddOrUpdateAsync(AddWorkspaceContract data);
    Task<IEnumerable<string>> GetWorkspacesIdsAsync();
    Task<T> GetWorkspaceAsync<T>(int togglWorkspaceId) where T : IMapFrom<Workspace>;
    Task<IEnumerable<T>> GetWorkspacesAsync<T>() where T : IMapFrom<Workspace>;
}
