using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.Workspaces;
using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Api.Controllers;

[Route("[controller]")]
public class WorkspacesController : AppControllerBase
{
    private readonly IWorkspaceRepository workspaceRepository;

    public WorkspacesController(IWorkspaceRepository workspaceRepository)
    {
        this.workspaceRepository = workspaceRepository;
    }

    [HttpGet("{togglWorkspaceId:int}")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator })]
    public async Task<IActionResult> Get(int togglWorkspaceId)
    {
        WorkspaceDetailContract result = await workspaceRepository.GetWorkspaceAsync<WorkspaceDetailContract>(togglWorkspaceId);
        return Ok(result);
    }

    [HttpGet]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<WorkspaceListContract> results = await workspaceRepository.GetWorkspacesAsync<WorkspaceListContract>();
        return Ok(results);
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator })]
    public async Task Add([FromBody] AddWorkspaceContract contract)
    {
        await workspaceRepository.AddOrUpdateAsync(contract);
    }

    [HttpPut("{togglWorkspaceId}")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator })]
    public async Task Update(int togglWorkspaceId, [FromBody] AddWorkspaceContract contract)
    {
        contract.ExternalId = togglWorkspaceId;
        await workspaceRepository.AddOrUpdateAsync(contract);
    }

    [HttpDelete("{id}")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator })]
    public async Task Delete(int id)
    {
        await workspaceRepository.RemoveWorkspaceIdAsync(id);
    }
}
