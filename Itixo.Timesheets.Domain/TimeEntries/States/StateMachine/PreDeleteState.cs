using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Base;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;

namespace Itixo.Timesheets.Domain.TimeEntries.States.StateMachine;

public class PreDeleteState : TimeEntryStateBase
{
    public override TimeEntryState StateType => TimeEntryState.Predeleted;

    public PreDeleteState(IdentityInfo identityInfo, string reason) : base(identityInfo, reason) { }

    public override void EnterState(TimeEntryStateContext context)
    {
        context.IsPredeleted = true;
        context.IsDraft = false;
        context.IsBan = false;
        context.IsApproved = false;
    }

    public override void ToApprove(ITimeEntryStateContext context, TransitionParams @params)
        => throw new UnsupportedTimeEntryStateTransitionException();

    public override void ToBan(ITimeEntryStateContext context, TransitionParams @params)
        => throw new UnsupportedTimeEntryStateTransitionException();

    public override void ToDraft(ITimeEntryStateContext context, TransitionParams @params)
        => throw new UnsupportedTimeEntryStateTransitionException();

    public override void ToPreDeleted(ITimeEntryStateContext context, TransitionParams @params)
        => throw new UnsupportedTimeEntryStateTransitionException();
}
