using System;
using System.Collections.Generic;

namespace Itixo.Timesheets.Contracts.TimeEntries;

public interface IReportsQueryFilter
{
    public DateTimeOffset FromDate { get; }
    public DateTimeOffset ToDate { get; }
    public List<int> ProjectIds { get; }
    public List<int> ClientIds { get; }
    public List<int> TimeTrackerIds { get; set; }
}

public class ReportsQueryFilter : IReportsQueryFilter
{
    public DateTimeOffset FromDate { get; set; }
    public DateTimeOffset ToDate { get; set; }
    public List<int> ProjectIds { get; set; } = new List<int>();
    public List<int> ClientIds { get; set; } = new List<int>();
    public List<int> TimeTrackerIds { get; set; } = new List<int>();
}
