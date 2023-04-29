using System;
using System.Collections.Generic;
using Itixo.Timesheets.Contracts.TimeEntries;
using Newtonsoft.Json;

namespace Itixo.Timesheets.Contracts.Invoices;

public class SummaryTimeEntriesInvoiceAssignmentParameter : ITimeTrackerAccountsReportsQueryFilter, ITimeEntriesQueryFilter
{
    public DateTimeOffset FromDate { get; set; }
    public DateTimeOffset ToDate { get; set; }
    public List<int> ProjectIds { get; set; } = new List<int>();
    public List<int> ClientIds { get; set; } = new List<int>();
    public List<int> TimeTrackerIds { get; set; } = new List<int>();
    public int TimeTrackerAccountId { get; set; }
    public string Number { get; set; }
    public bool IncludeApproved { get; set; }
    public bool IncludeDrafts { get; set; }
    public bool IncludeBans { get; set; }
    public string SummaryDurationAllEntries { get; set; } = TimeSpan.Zero.ToString();
    public string SummaryDurationApproves { get; set; } = TimeSpan.Zero.ToString();
    public string SummaryDurationDrafts { get; set; } = TimeSpan.Zero.ToString();
    public string SummaryDurationBans { get; set; } = TimeSpan.Zero.ToString();

    /// <summary>
    /// Neccessery for deserialization
    /// </summary>
    public SummaryTimeEntriesInvoiceAssignmentParameter() { }

    public SummaryTimeEntriesInvoiceAssignmentParameter(ITimeTrackerAccountsReportsQueryFilter timeTrackerAccountsReposQueryFilter)
    {
        ProjectIds = timeTrackerAccountsReposQueryFilter.ProjectIds;
        ClientIds = timeTrackerAccountsReposQueryFilter.ClientIds;
        FromDate = timeTrackerAccountsReposQueryFilter.FromDate;
        ToDate = timeTrackerAccountsReposQueryFilter.ToDate;
        TimeTrackerAccountId = timeTrackerAccountsReposQueryFilter.TimeTrackerAccountId;
    }

    public string ToLogString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
