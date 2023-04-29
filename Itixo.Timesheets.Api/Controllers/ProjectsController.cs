using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.Projects;
using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Api.Controllers;

[Route("[controller]")]
public class ProjectsController : AppControllerBase
{
    private readonly IProjectRepository projectRepository;

    public ProjectsController(IProjectRepository projectRepository)
    {
        this.projectRepository = projectRepository;
    }

    [HttpGet]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<IActionResult> GetAll([FromBody] IEnumerable<int> clientIds)
    {
        IEnumerable<ProjectBaseContract> projects = await projectRepository.GetByClientIdsOrAllAsync<ProjectBaseContract>(clientIds);
        return Ok(projects);
    }

    [HttpGet]
    [Route("BaseList")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser, RoleDefinition.TimesheetAccess })]
    public async Task<ActionResult<ProjectBaseContract>> GetAll()
    {
        IEnumerable<ProjectBaseContract> projects = await projectRepository.GetProjectsAsync<ProjectBaseContract>();
        return Ok(projects);
    }

    [HttpGet("{togglProjectId:long}")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<IActionResult> GetById(long togglProjectId)
    {
        ProjectDetailContract project = await projectRepository.GetProjectAsync(togglProjectId);
        return Ok(project);
    }

    [HttpDelete("{togglProjectId:long}")]
    [Authorize(Roles = RoleDefinition.TimeEntriesAdministrator)]
    public async Task<IActionResult> Delete(long togglProjectId)
    {
        await projectRepository.RemoveProjectAsync(togglProjectId);
        return Ok();
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<IActionResult> AddOrUpdate([FromBody] ProjectContract projectContract)
    {
        await projectRepository.AddOrUpdateProjectAsync(projectContract);
        return Ok();
    }

    [HttpGet]
    [Route("GetByName")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<ActionResult<ProjectContract>> GetByName([FromQuery] string projectName)
    {
        ProjectContract project = await projectRepository.GetByNameAsync(projectName);
        return Ok(project);
    }
}
