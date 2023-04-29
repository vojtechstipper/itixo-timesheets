using Itixo.Timesheets.Contracts.TimeTrackers;
using MediatR;

namespace Itixo.Timesheets.Application.TimeTrackers.Queries.ByTypeQuery;

public class TimeTrackerByTypeQueryResponse : IRequest<TimeTrackerByTypeQuery>
{
    public TimeTrackerContract TimeTracker { get; set; }
}
