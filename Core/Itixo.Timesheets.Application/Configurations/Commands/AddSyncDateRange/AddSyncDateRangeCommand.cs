using MediatR;

namespace Itixo.Timesheets.Application.Configurations.Commands.AddSyncDateRange;

public class AddSyncDateRangeCommand : IRequest<AddSyncDateRangeResponse>
{
    public int StartSyncBusinessDaysAgoValue { get; set; }
    public int StopSyncBusinessDaysAgoValue { get; set; }
}
