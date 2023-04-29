namespace Itixo.Timesheets.Shared.ConstantObjects;

public class Endpoints
{
    public const string GetSyncDateRange = "Synchronization/GetSyncDateRange";
    public const string AddSyncDateRange = "Synchronization/AddSyncDateRange";
    public const string UpdateSyncDateRange = "Synchronization/UpdateSyncDateRange";
    public const string GetLogRecords = "Synchronization/GetLogRecords";
    public const string GetSyncLock = "Synchronization/GetLock";

    public const string BanTimeEntry = "TimeEntries/Ban";
    public const string ApproveTimeEntry = "TimeEntries/Approve";
    public const string AddTimeEntry = "TimeEntries/Add";
    public const string DeleteTimeEntry = "TimeEntries/Delete";

    public const string FilteredTimeEntries = nameof(FilteredTimeEntries);
    public const string Projects = nameof(Projects);
    public const string TimeTrackerAccounts = nameof(TimeTrackerAccounts);
    public const string TimeTrackerAccountsAddOrUpdate = nameof(TimeTrackerAccounts) + "/AddOrUpdate";
    public const string TimeTrackerAccounts_OnlyApplicationAccounts = "TimeTrackerAccounts/OnlyApplicationAccounts";
    public const string ProjectBaseList = "Projects/BaseList";
    public const string Reports = nameof(Reports);
    public const string ReportsSummary = "Reports/summary";
    public const string Workspaces = nameof(Workspaces);
    public const string TogglUsersWorkspaces = nameof(TogglUsersWorkspaces);
    public const string UserByUsername = nameof(UserByUsername);
    public const string Clients = nameof(Clients);
    public const string AssignInvoiceToSummaries = "Invoices/AssignInvoiceToSummaries";
    public const string AssignInvoiceToTimeEntries = "Invoices/AssignInvoiceToTimeEntries";
    public const string FilteredTimeEntriesPagingInfo = nameof(FilteredTimeEntriesPagingInfo);
    public const string TimeEntryVersions = nameof(TimeEntryVersions);
    public const string TimeTrackers = nameof(TimeTrackers);
    public const string TimeTrackersByType = nameof(TimeTrackers) + "/ByType";
    public const string TimeEntryStateChanges = nameof(TimeEntryStateChanges);
    public const string GetCurrentIdentityInfo = "IdentityInfos/GetCurrent";
    public const string UpdateCurrentIdentityInfo = "IdentityInfos/UpdateCurrent";
    public const string FilteredPreDeleteTimeEntries = nameof(FilteredPreDeleteTimeEntries);
    public const string RunSynchronization = "synchronization/run";
}
