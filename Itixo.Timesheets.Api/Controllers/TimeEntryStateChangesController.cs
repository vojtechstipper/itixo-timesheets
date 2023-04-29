using Itixo.Timesheets.Application.TimeEntries.States.Queries.StateChangesByTimeEntryId;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Api.Controllers;

[Route("[controller]")]
public class TimeEntryStateChangesController : AppControllerBase
{
    public async Task<IActionResult> Get([FromBody] StateChangesByTimeEntryIdQuery query)
    {
        StateChangesByTimeEntryIdResponse result = await Mediator.Send(query);
        return Ok(result);
    }
}
