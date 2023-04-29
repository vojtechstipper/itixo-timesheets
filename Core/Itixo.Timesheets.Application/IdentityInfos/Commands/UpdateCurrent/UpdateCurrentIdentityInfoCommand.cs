using MediatR;

namespace Itixo.Timesheets.Application.IdentityInfos.Commands.UpdateCurrent;

public class UpdateCurrentIdentityInfoCommand : IRequest<Unit>
{
    public string Email { get; set; }
}