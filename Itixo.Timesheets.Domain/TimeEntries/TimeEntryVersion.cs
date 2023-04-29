using System;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Domain.TimeEntries;

public class TimeEntryVersion : IEntity<int>
{
    public int Id { get; set; }
    public TimeEntryParams TimeEntryParams { get; internal set; }
    public TimeEntry TimeEntry { get; internal set; }
    public TimeEntryVersion PreviousTimeEntryVersion { get; internal set; }
    public DateTimeOffset ImportedDate { get; internal set; }

    public static TimeEntryVersion Create(
        TimeEntryParams timeEntryParams,
        TimeEntry timeEntry,
        TimeEntryVersion previous,
        DateTimeOffset importedDate)
    {
        return new TimeEntryVersion
        {
            TimeEntryParams = timeEntryParams,
            TimeEntry = timeEntry,
            PreviousTimeEntryVersion = previous,
            ImportedDate = importedDate
        };
    }
}
