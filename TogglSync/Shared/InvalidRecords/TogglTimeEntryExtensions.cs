using TogglSyncShared.DataObjects;

namespace TogglSyncShared.InvalidRecords;

public static class TogglTimeEntryExtensions
{
    public static bool IsProjectInvalid(this TogglTimeEntry togglTimeEntry)
        => togglTimeEntry.ProjectId == null;
    public static bool IsTrackingFinished(this TogglTimeEntry togglTimeEntry)
        => togglTimeEntry.Stop != null;
    public static bool IsDescriptionEmpty(this TogglTimeEntry togglTimeEntry)
        => string.IsNullOrWhiteSpace(togglTimeEntry.Description);
}
