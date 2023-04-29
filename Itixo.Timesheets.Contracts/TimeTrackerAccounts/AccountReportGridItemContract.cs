using System;
using Itixo.Timesheets.Shared.Extensions;

namespace Itixo.Timesheets.Contracts.TimeTrackerAccounts;

public class AccountReportGridItemContract
{
    public int TimeTrackerAccountId { get; set; }
    public string Username { get; set; }

    public TimeSpan SummaryDurationAllEntries { get; set; }
    public TimeSpan SummaryDurationBans { get; set; }
    public TimeSpan SummaryDurationDrafts { get; set; }
    public TimeSpan SummaryDurationApproves { get; set; }

    public string SummaryDurationAllEntriesFormmated => SummaryDurationAllEntries.ToCustomHoursFormat();
    public string SummaryDurationBansFormmated => SummaryDurationBans.ToCustomHoursFormat();
    public string SummaryDurationDraftsFormmated => SummaryDurationDrafts.ToCustomHoursFormat();
    public string SummaryDurationApprovesFormmated => SummaryDurationApproves.ToCustomHoursFormat();
}
