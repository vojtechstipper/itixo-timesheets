using System;
using System.Collections.Generic;

namespace Itixo.Timesheets.Contracts.TimeEntries;

public interface ITimeTrackerAccountsReportsQueryFilter : IReportsQueryFilter
{
    int TimeTrackerAccountId { get; }
}

public class TimeTrackerAccountsReportsQueryFilter : ITimeTrackerAccountsReportsQueryFilter
{
    public int TimeTrackerAccountId { get; set; }
    public DateTimeOffset FromDate { get; set; }
    public DateTimeOffset ToDate { get; set; }
    public List<int> ProjectIds { get; set; }
    public List<int> ClientIds { get; set; }
    public List<int> TimeTrackerIds { get; set; }

    public TimeTrackerAccountsReportsQueryFilter(IReportsQueryFilter baseFilter)
    {
        ProjectIds = baseFilter.ProjectIds;
        ClientIds = baseFilter.ClientIds;
        FromDate = baseFilter.FromDate;
        TimeTrackerIds = baseFilter.TimeTrackerIds;
        ToDate = baseFilter.ToDate;
    }
}
