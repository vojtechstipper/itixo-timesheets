using Itixo.Timesheets.Shared.Extensions;
using System;

namespace Itixo.Timesheets.Contracts.TimeTrackerAccounts;

public class AccountReportGridSummaryContract
{
    public TimeSpan TotalSummaryAllEntries { get; set; }
    public TimeSpan TotalSummaryBansEntries { get; set; }
    public TimeSpan TotalSummaryDraftsEntries { get; set; }
    public TimeSpan TotalSummaryApprovesEntries { get; set; }
    public string TotalSummaryDurationAllEntriesFormmated => TotalSummaryAllEntries.ToCustomHoursFormat();
    public string TotalSummaryDurationBansFormmated => TotalSummaryBansEntries.ToCustomHoursFormat();
    public string TotalSummaryDurationDraftsFormmated => TotalSummaryDraftsEntries.ToCustomHoursFormat();
    public string TotalSummaryDurationApprovesFormmated => TotalSummaryApprovesEntries.ToCustomHoursFormat();
}
