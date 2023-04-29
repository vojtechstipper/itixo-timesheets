using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itixo.Timesheets.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("[controller]/[action]")]
public class AppControllerBase : ControllerBase
{
    private IMediator mediator;

    protected IMediator Mediator => mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator));
}
