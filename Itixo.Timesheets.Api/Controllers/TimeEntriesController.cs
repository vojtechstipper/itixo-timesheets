using Itixo.Timesheets.Application.TimeEntries.Commands.AddTimeEntry;
using Itixo.Timesheets.Application.TimeEntries.Commands.ApproveTimeEntry;
using Itixo.Timesheets.Application.TimeEntries.Commands.BanTimeEntry;
using Itixo.Timesheets.Application.TimeEntries.Commands.DeleteTimeEntry;
using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Api.Controllers;

public class TimeEntriesController : AppControllerBase
{
    [HttpPost]
    [Authorize(Roles = RoleDefinition.TimeEntriesAdministrator)]
    public async Task<IActionResult> Approve([FromBody] ApproveTimeEntryCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = RoleDefinition.TimeEntriesAdministrator)]
    public async Task<IActionResult> Ban([FromBody] BanTimeEntryCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPost]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<IActionResult> Add(AddTimeEntryCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpPost]
    [Authorize(Roles = RoleDefinition.TimeEntriesAdministrator)]
    public async Task<IActionResult> Delete(DeleteTimeEntryCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}
