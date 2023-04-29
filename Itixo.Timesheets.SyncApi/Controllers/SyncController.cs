using AsyncAwaitBestPractices;
using Itixo.Timesheets.Shared.Roles;
using Itixo.Timesheets.SyncApi.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Itixo.Timesheets.SyncApi.Controllers;

[Route("synchronization")]
public class SyncController : AppControllerBase
{
    private readonly ILogger<SyncController> logger;

    public SyncController(ILogger<SyncController> logger)
    {
        this.logger = logger;
    }

    [HttpPost("run")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess })]
    public async Task<IActionResult> RunSyncAsync([FromBody] RunSynchronizationCommand command)
    {
        Mediator.Send(command).SafeFireAndForget(x => logger.LogInformation(x.Message));
        return Ok();
    }
}
