using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using System;
using System.Collections.Generic;
using TogglSyncShared.DataObjects;

namespace TogglSyncShared.TimeEntrySync;

public class TogglTimeEntryDataTransformer
{
    private readonly DateTimeOffset dateTimeNow;

    public TogglTimeEntryDataTransformer(DateTimeOffset dateTimeNow)
    {
        this.dateTimeNow = dateTimeNow;
    }

    public List<TimeEntryContract> Transform(List<TogglTimeEntry> togglTimeEntries, AccountSyncContract accountSyncContract)
    {
        var timeEntryContracts = new List<TimeEntryContract>();

        foreach (TogglTimeEntry togglTimeEntry in togglTimeEntries)
        {
            var timeEntryContract = new TimeEntryContract
            {
                ExternalProjectId = togglTimeEntry.ProjectId,
                ExternalTimeEntryId = togglTimeEntry.Id,
                Description = togglTimeEntry.Description,
                CreatedDate = togglTimeEntry.UpdatedOn ?? dateTimeNow,
                LastModifiedDate = togglTimeEntry.UpdatedOn ?? dateTimeNow,
                StartTime = togglTimeEntry.Start ?? throw new Exception("Start value cannot be null"),
                StopTime = togglTimeEntry.Stop ?? throw new Exception("Stop value cannot be null"),
                TaskName = togglTimeEntry.TaskName,
                TimeTrackerExternalId = accountSyncContract.ExternalId
            };

            timeEntryContracts.Add(timeEntryContract);
        }

        return timeEntryContracts;
    }
}
