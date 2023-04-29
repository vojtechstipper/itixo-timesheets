using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Domain.TimeTrackers;

namespace Itixo.Timesheets.Application.TimeEntries.Services;

public interface ITimeEntryComparer
{
    bool EqualsTo(TimeEntry timeEntry, TimeEntryContract parameters, TimeTrackerAccount timeTrackerAccount);
}
