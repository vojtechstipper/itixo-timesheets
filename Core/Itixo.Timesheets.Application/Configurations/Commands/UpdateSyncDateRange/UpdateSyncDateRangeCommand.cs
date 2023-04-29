using MediatR;
using MediatR.Extensions.AttributedBehaviors;

namespace Itixo.Timesheets.Application.Configurations.Commands.UpdateSyncDateRange;

[MediatRBehavior(typeof(SyncDateRangeExistenceValidator))]
public class UpdateSyncDateRangeCommand : IRequest<Unit>
{
    public int StartSyncBusinessDaysAgoId { get; set; }
    public int StopSyncBusinessDaysAgoId { get; set; }
    public int StartSyncBusinessDaysAgoValue { get; set; }
    public int StopSyncBusinessDaysAgoValue { get; set; }
}
