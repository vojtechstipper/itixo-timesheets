using AutoMapper;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.Workspaces;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Persistence;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Exceptions;
using Itixo.Timesheets.Shared.Resources;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class WorkspaceRepository : AppRepositoryBase<Workspace, int>, IWorkspaceRepository
{
    private readonly IMapper mapper;

    public WorkspaceRepository(IDbContext dbContext, IMapper mapper) : base(dbContext) {
        this.mapper = mapper;
    }

    public async Task<string> RemoveWorkspaceIdAsync(int togglWorkspaceId)
    {
        Workspace workspace = await dbContext.Workspaces
            .FirstOrDefaultAsync(f => f.ExternalId == togglWorkspaceId);

        if (workspace == null)
        {
            throw new NotFoundException(
                string.Format(
                Texts.WorkspaceRepository_NotFoundWorkspace_Workspace_Id_Not_Found,
                togglWorkspaceId));
        }

        dbContext.Workspaces.Remove(workspace);
        await dbContext.SaveChangesAsync();

        return Texts.Repository_RemoveMessage_Record_Was_Successfully_Removed;
    }

    public async Task<string> AddOrUpdateAsync(AddWorkspaceContract data)
    {
        Workspace workspace = await dbContext.Workspaces
            .FirstOrDefaultAsync(f => f.ExternalId == data.ExternalId);

        if (workspace == null)
        {
            workspace = new Workspace {Name = data.WorkspaceName, ExternalId = data.ExternalId};
            await InsertWorkspace(workspace);
            return Texts.Repository_AddMessage_Record_Was_Created;
        }
        else
        {
            workspace.Name = data.WorkspaceName;
            workspace.ExternalId = data.ExternalId;
            await UpdateAsync(workspace);
            return Texts.Repository_UpdateMessage_Record_Was_Updated;
        }
    }

    public async Task<IEnumerable<string>> GetWorkspacesIdsAsync()
        => await dbContext.Workspaces.Select(s => s.ExternalId.ToString()).ToListAsync();

    public async Task<T> GetWorkspaceAsync<T>(int togglWorkspaceId)
    where T : IMapFrom<Workspace>
    {
        Workspace workspace = await dbContext.Workspaces
            .FirstOrDefaultAsync(f => f.ExternalId == togglWorkspaceId);

        if (workspace == null)
        {
            throw new NotFoundException(string.Format(Texts.WorkspaceRepository_NotFoundWorkspace_Workspace_Id_Not_Found, togglWorkspaceId));
        }

        return mapper.Map<T>(workspace);
    }

    public async Task<IEnumerable<T>> GetWorkspacesAsync<T>()
    where T : IMapFrom<Workspace>
    {
        return await mapper.ProjectTo<T>(dbContext.Workspaces).ToListAsync();
    }

    public async Task<IEnumerable<WorkspaceListContract>> GetWorkspacesAsync()
    {
        return await dbContext.Workspaces
            .Select(s => new WorkspaceListContract {ExternalId = s.ExternalId, Name = s.Name, Id = s.Id})
            .ToListAsync();
    }

    private async Task UpdateAsync(Workspace workspace)
    {
        dbContext.Workspaces.Update(workspace);
        await dbContext.SaveChangesAsync();
    }

    private async Task InsertWorkspace(Workspace workspace)
    {
        await dbContext.Workspaces.AddAsync(workspace);
        await dbContext.SaveChangesAsync();
    }
}
