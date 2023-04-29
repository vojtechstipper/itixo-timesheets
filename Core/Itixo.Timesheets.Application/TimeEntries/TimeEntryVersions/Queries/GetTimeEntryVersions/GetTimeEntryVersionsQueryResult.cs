using System.Collections.Generic;
using Itixo.Timesheets.Contracts.TimeEntries.Versions;

namespace Itixo.Timesheets.Application.TimeEntries.TimeEntryVersions.Queries.GetTimeEntryVersions;

public class GetTimeEntryVersionsQueryResult
{
    public List<TimeEntryVersionContract> TimeEntryVersions { get; set; } = new List<TimeEntryVersionContract>();
}
