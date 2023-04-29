using System.Collections.Generic;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Base;

namespace Itixo.Timesheets.Domain.TimeEntries.States.StateMachine;

public interface ITimeEntryStateContext
{
    bool IsApproved { get; }
    bool IsDraft { get; }
    bool IsBan { get; }
    bool IsPredeleted { get; }
    List<TimeEntryStateChange> TimeEntryStateChanges { get; }
    ITimeEntryState CurrentState { get; }
    void Transition(ITimeEntryState timeEntryState);
}

public class TimeEntryStateContext : ITimeEntryStateContext
{
    public bool IsApproved { get; set; }
    public bool IsDraft { get; set; }
    public bool IsBan { get; set; }
    public bool IsPredeleted { get; set; }
    public List<TimeEntryStateChange> TimeEntryStateChanges { get; } = new List<TimeEntryStateChange>();
    public ITimeEntryState CurrentState { get; set; }

    public TimeEntryStateContext()
    {
        CurrentState = new NoneState(new IdentityInfo(), string.Empty);
    }
    public void Transition(ITimeEntryState timeEntryState)
    {
        var timeEntryStateChange = TimeEntryStateChange
            .Create(CurrentState.StateType, timeEntryState.StateType, timeEntryState.IdentityInfo, timeEntryState.Reason);
        TimeEntryStateChanges.Add(timeEntryStateChange);
        CurrentState = timeEntryState;
        CurrentState.EnterState(this);
    }
}
