using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;

namespace Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Base;

public abstract class TimeEntryStateBase : ITimeEntryState
{
    public string Reason { get; }
    public IdentityInfo IdentityInfo { get; }
    public abstract TimeEntryState StateType { get; }


    protected TimeEntryStateBase(IdentityInfo identityInfo, string reason)
    {
        IdentityInfo = identityInfo;
        Reason = reason;
    }

    public abstract void EnterState(TimeEntryStateContext context);

    public abstract void ToApprove(ITimeEntryStateContext context, TransitionParams @params);

    public abstract void ToBan(ITimeEntryStateContext context, TransitionParams @params);

    public abstract void ToDraft(ITimeEntryStateContext context, TransitionParams @params);

    public abstract void ToPreDeleted(ITimeEntryStateContext context, TransitionParams @params);
}
