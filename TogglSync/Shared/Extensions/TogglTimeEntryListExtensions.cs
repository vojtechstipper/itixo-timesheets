using System;
using System.Collections.Generic;
using System.Linq;
using TogglSyncShared.DataObjects;

namespace TogglSyncShared.Extensions;

public static class TogglTimeEntryListExtensions
{
    public static List<TogglTimeEntry> RemoveInvalidRecords(this List<TogglTimeEntry> togglUsersTimeEntries, Predicate<TogglTimeEntry> conditionForRemoval)
    {
        togglUsersTimeEntries.RemoveAll(conditionForRemoval);
        return togglUsersTimeEntries;
    }

    public static List<TogglTimeEntry> AssignFictionalProjectWhenNoProject(this List<TogglTimeEntry> togglUsersTimeEntries, string fictionalProjectExternalId)
    {
        if (string.IsNullOrEmpty(fictionalProjectExternalId))
        {
            return togglUsersTimeEntries;
        }

        foreach (var timeEntry in togglUsersTimeEntries.Where(x => x.ProjectId == null))
        {
            timeEntry.ProjectId = Convert.ToInt64(fictionalProjectExternalId);
        }

        return togglUsersTimeEntries;
    }
}
