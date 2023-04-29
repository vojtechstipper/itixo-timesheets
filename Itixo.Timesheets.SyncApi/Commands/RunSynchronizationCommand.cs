using MediatR;

namespace Itixo.Timesheets.SyncApi.Commands;

public class RunSynchronizationCommand : IRequest<Unit>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string IdentityExternalId { get; set; }
    public bool ManualRun { get; set; }
}
