using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Itixo.Timesheets.SyncApi.Controllers;

[ApiController]
[Produces("application/json")]
public abstract class AppControllerBase : ControllerBase
{
    private IMediator mediator;
    protected IMediator Mediator => mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator));
}
