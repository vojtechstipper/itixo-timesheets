using System.Collections.Generic;
using Itixo.Timesheets.Contracts.TimeTrackers;

namespace Itixo.Timesheets.Application.TimeTrackers.Queries.AllQuery;

public class TimeTrackerQueryResponse
{
    public List<TimeTrackerContract> TimeTrackers { get; set; } = new List<TimeTrackerContract>();
}