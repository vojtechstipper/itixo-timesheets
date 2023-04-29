namespace Itixo.Timesheets.Shared.Roles;

public class RoleDefinition
{
    public const string AdminRoleKey = "Admin";
    public const string TimeSheetAppKey = "TimeSheetApp";

    public const string SynchronizatorAccess = "access_as_sync_application";
    public const string TimesheetAccess = "access_as_timesheets_app";
    public const string TimeEntriesAdministrator = "TimeEntries.Administrator";
    public const string TimeEntriesUser = "TimeEntries.User";

    public string Key { get; set; }
    public string RoleName { get; set; }
}
