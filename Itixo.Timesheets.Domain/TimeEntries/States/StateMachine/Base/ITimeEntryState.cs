using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;

namespace Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Base;

public interface ITimeEntryState
{
    void EnterState(TimeEntryStateContext context);
    void ToApprove(ITimeEntryStateContext context, TransitionParams @params);
    void ToBan(ITimeEntryStateContext context, TransitionParams @params);
    void ToDraft(ITimeEntryStateContext context, TransitionParams @params);
    void ToPreDeleted(ITimeEntryStateContext context, TransitionParams @params);
    TimeEntryState StateType { get; }
    IdentityInfo IdentityInfo { get; }
    string Reason { get; }
}
