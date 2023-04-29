using System.Threading.Tasks;
using Itixo.Timesheets.Application.TimeTrackers.Queries.AllQuery;
using Itixo.Timesheets.Application.TimeTrackers.Queries.ByTypeQuery;
using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Mvc;

namespace Itixo.Timesheets.Api.Controllers;

[Route("[controller]")]
public class TimeTrackersController : AppControllerBase
{
    [HttpGet]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<ActionResult<TimeTrackerQueryResponse>> Get()
    {
        var query = new TimeTrackersQuery();
        return Ok(await Mediator.Send(query));
    }

    [HttpGet]
    [Route("ByType")]
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator, RoleDefinition.TimeEntriesUser })]
    public async Task<ActionResult<TimeTrackerByTypeQueryResponse>> GetByType(TimeTrackerByTypeQuery query)
    {
        return Ok(await Mediator.Send(query));
    }
}
