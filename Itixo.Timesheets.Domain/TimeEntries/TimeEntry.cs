using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;
using Itixo.Timesheets.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Itixo.Timesheets.Domain.TimeTrackers;

namespace Itixo.Timesheets.Domain.TimeEntries;

public class TimeEntry : IEntity<int>
{
    public int Id { get; set; }
    public TimeEntryParams TimeEntryParams { get; internal set; } = new TimeEntryParams();

    public TimeEntryStateContext StateContext { get; set; } = new TimeEntryStateContext();

    public int TimeTrackerAccountId { get; set; }
    public TimeTrackerAccount TimeTrackerAccount { get; set; }

    public int? InvoiceId { get; set; }
    public virtual Invoice Invoice { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset ImportedDate { get; set; }
    public List<TimeEntryVersion> TimeEntryVersions { get; } = new List<TimeEntryVersion>();

    public void Update(TimeEntryParams timeEntryParams, TimeTrackerAccount account, DateTimeOffset importedDate)
    {
        TimeEntryVersion previousTimeEntryVersion = TimeEntryVersions.LastOrDefault();

        var timeEntryVersion = TimeEntryVersion.Create(timeEntryParams, this, previousTimeEntryVersion, importedDate);
        TimeEntryVersions.Add(timeEntryVersion);

        TimeEntryParams = timeEntryParams;
        TimeTrackerAccount = account;
        ImportedDate = importedDate;
    }


    public static TimeEntry CreateDraft(TimeEntryParams timeEntryParams, TimeTrackerAccount account, DateTimeOffset importedDate, TransitionParams @params)
    {
        TimeEntry timeEntry = Create(timeEntryParams, account, importedDate);
        timeEntry.StateContext.CurrentState.ToDraft(timeEntry.StateContext, @params);
        return timeEntry;
    }

    public static TimeEntry CreateApproved(TimeEntryParams timeEntryParams, TimeTrackerAccount account, DateTimeOffset importedDate, TransitionParams @params)
    {
        TimeEntry timeEntry = Create(timeEntryParams, account, importedDate);
        timeEntry.StateContext.CurrentState.ToApprove(timeEntry.StateContext, @params);
        return timeEntry;
    }

    private static TimeEntry Create(TimeEntryParams timeEntryParams, TimeTrackerAccount account, DateTimeOffset importedDate)
    {
        var timeEntry = new TimeEntry
        {
            TimeEntryParams = timeEntryParams,
            TimeTrackerAccount = account,
            ImportedDate = importedDate,
            CreatedDate = DateTimeOffset.UtcNow
        };
        var timeEntryVersion = TimeEntryVersion.Create(timeEntryParams, timeEntry, null, importedDate);
        timeEntry.TimeEntryVersions.Add(timeEntryVersion);
        return timeEntry;
    }
}
