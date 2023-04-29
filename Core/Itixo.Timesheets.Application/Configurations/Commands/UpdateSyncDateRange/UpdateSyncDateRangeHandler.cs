using Itixo.Timesheets.Application.Configurations.Commands.UpdateConfiguration;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.Configurations.Commands.UpdateSyncDateRange;

public class UpdateSyncDateRangeHandler : IRequestHandler<UpdateSyncDateRangeCommand, Unit>
{
    private readonly IMediator mediator;

    public UpdateSyncDateRangeHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<Unit> Handle(UpdateSyncDateRangeCommand request, CancellationToken cancellationToken)
    {
        var startSyncBusinessDaysAgoCommand = UpdateConfigurationCommand.CreateStartSyncBusinessDaysAgoCommand(request);
        await mediator.Send(startSyncBusinessDaysAgoCommand, cancellationToken);

        var stopSyncBusinessDaysAgoCommand = UpdateConfigurationCommand.CreateStopSyncBusinessDaysAgoCommand(request);
        await mediator.Send(stopSyncBusinessDaysAgoCommand, cancellationToken);

        return Unit.Value;
    }
}
