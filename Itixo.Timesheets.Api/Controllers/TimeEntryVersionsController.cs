using Itixo.Timesheets.Shared.Roles;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.TimeEntries.TimeEntryVersions.Queries.GetTimeEntryVersions;

namespace Itixo.Timesheets.Api.Controllers;

[Route("[controller]")]
public class TimeEntryVersionsController : AppControllerBase
{
    [RolesAuthorize(new[] { RoleDefinition.TimeEntriesAdministrator,  RoleDefinition.TimeEntriesUser })]
    public async Task<ActionResult<GetTimeEntryVersionsQueryResult>> Get(GetTimeEntryVersionsQuery query)
    {
        return await Mediator.Send(query);
    }
}
