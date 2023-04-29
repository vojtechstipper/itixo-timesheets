using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Domain;
using System;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Domain.TimeTrackers;

namespace Itixo.Timesheets.Application.TimeEntries.Factories;

public interface ITimeEntryBaseFactory
{
    TimeEntry Create(TimeEntryContract timeEntryContract, Project project);
    TimeEntry Create(TimeEntryContract timeEntryContract, DateTime now, Project project, TimeTrackerAccount timeTrackerAccount);
    void Populate(TimeEntry timeEntry, TimeEntryContract timeEntryContract, in DateTime now);
}
