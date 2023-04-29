using Itixo.Timesheets.Application.IdentityInfos.Queries.GetCurrentIdentityInfo;
using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.IdentityInfos.Commands.UpdateCurrent;

namespace Itixo.Timesheets.Api.Controllers;

public class IdentityInfosController : AppControllerBase
{
    [HttpGet]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess, RoleDefinition.TimeEntriesUser })]
    public async Task<IActionResult> GetCurrent()
    {
        var query = new GetCurrentIdentityInfoQuery();
        GetCurrentIdentityInfoResponse response = await Mediator.Send(query);
        return Ok(response);
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.SynchronizatorAccess, RoleDefinition.TimeEntriesUser })]
    public async Task<IActionResult> UpdateCurrent([FromBody] UpdateCurrentIdentityInfoCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}
