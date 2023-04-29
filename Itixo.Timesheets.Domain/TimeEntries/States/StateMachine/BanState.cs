using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Base;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;

namespace Itixo.Timesheets.Domain.TimeEntries.States.StateMachine;

public class BanState : TimeEntryStateBase
{
    public override TimeEntryState StateType => TimeEntryState.Ban;

    public BanState(IdentityInfo identityInfo, string reason) : base(identityInfo, reason) { }

    public override void EnterState(TimeEntryStateContext context)
    {
        context.IsBan = true;
        context.IsDraft = false;
        context.IsApproved = false;
        context.IsPredeleted = false;
    }

    public override void ToApprove(ITimeEntryStateContext context, TransitionParams @params)
        => context.Transition(new ApproveState(@params.IdentityInfo, @params.Reason));

    public override void ToBan(ITimeEntryStateContext context, TransitionParams @params)
        => throw new UnsupportedTimeEntryStateTransitionException();

    public override void ToDraft(ITimeEntryStateContext context, TransitionParams @params)
        => throw new UnsupportedTimeEntryStateTransitionException();

    public override void ToPreDeleted(ITimeEntryStateContext context, TransitionParams @params)
        => context.Transition(new PreDeleteState(@params.IdentityInfo, @params.Reason));
}
