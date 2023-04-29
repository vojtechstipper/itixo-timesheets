using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Configurations.Commands.AddConfiguration;
using MediatR;

namespace Itixo.Timesheets.Application.Configurations.Commands.AddSyncDateRange;

public class AddSyncDateRangeHandler : IRequestHandler<AddSyncDateRangeCommand, AddSyncDateRangeResponse>
{
    private readonly IMediator mediator;

    public AddSyncDateRangeHandler(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task<AddSyncDateRangeResponse> Handle(AddSyncDateRangeCommand request, CancellationToken cancellationToken)
    {
        var startSyncBusinessDaysAgoConfigurationCommand
            = AddConfigurationCommand.CreateStartSyncBusinessDaysAgoCommand(request.StartSyncBusinessDaysAgoValue);

        AddConfigurationResponse startSyncBusinessDaysAgoConfigurationResponse
            = await mediator.Send(startSyncBusinessDaysAgoConfigurationCommand, cancellationToken);

        var stopSyncBusinessDaysAgoConfigurationCommand
            = AddConfigurationCommand.CreateStopSyncBusinessDaysAgoCommand(request.StopSyncBusinessDaysAgoValue);

        AddConfigurationResponse stopSyncBusinessDaysAgoConfigurationResponse
            = await mediator.Send(stopSyncBusinessDaysAgoConfigurationCommand, cancellationToken);

        return new AddSyncDateRangeResponse
        {
            StartSyncBusinessDaysFromConfigurationId = startSyncBusinessDaysAgoConfigurationResponse.ConfigurationId,
            StopSyncBusinessDaysFromConfigurationId = stopSyncBusinessDaysAgoConfigurationResponse.ConfigurationId
        };
    }
}
