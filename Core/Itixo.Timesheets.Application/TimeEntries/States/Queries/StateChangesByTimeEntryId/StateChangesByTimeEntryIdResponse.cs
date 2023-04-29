using System.Collections.Generic;
using Itixo.Timesheets.Contracts.TimeEntries.States;

namespace Itixo.Timesheets.Application.TimeEntries.States.Queries.StateChangesByTimeEntryId;

public class StateChangesByTimeEntryIdResponse
{
    public List<TimeEntryStateChangeContract> TimeEntryStateChanges { get; set; }
        = new List<TimeEntryStateChangeContract>();
}
