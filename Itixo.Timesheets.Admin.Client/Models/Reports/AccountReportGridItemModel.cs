namespace Itixo.Timesheets.Admin.Client.Models.Reports;

public class AccountReportGridItemModel
{
    public int TimeTrackerAccountId { get; set; }
    public string Username { get; set; }
    public string SummaryDurationAllEntriesFormmated { get; set; }
    public string SummaryDurationBansFormmated { get; set; }
    public string SummaryDurationDraftsFormmated { get; set; }
    public string SummaryDurationApprovesFormmated { get; set; }
}
