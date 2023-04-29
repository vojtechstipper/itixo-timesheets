using Itixo.Timesheets.Shared.Enums;
using MediatR;

namespace Itixo.Timesheets.Application.TimeTrackers.Queries.ByTypeQuery;

public class TimeTrackerByTypeQuery : IRequest<TimeTrackerByTypeQueryResponse>
{
    public TimeTrackerType TimeTrackerType { get; set; }
}
