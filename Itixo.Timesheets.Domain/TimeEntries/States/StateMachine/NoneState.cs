﻿using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Base;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;

namespace Itixo.Timesheets.Domain.TimeEntries.States.StateMachine;

public class NoneState : TimeEntryStateBase
{
    public override TimeEntryState StateType => TimeEntryState.None;

    public NoneState(IdentityInfo identityInfo, string reason) : base(identityInfo, reason) { }

    public override void EnterState(TimeEntryStateContext context)
    {
        context.IsPredeleted = false;
        context.IsDraft = false;
        context.IsBan = false;
        context.IsApproved = false;
    }

    public override void ToApprove(ITimeEntryStateContext context, TransitionParams @params)
        => context.Transition(new ApproveState(@params.IdentityInfo, @params.Reason));

    public override void ToBan(ITimeEntryStateContext context, TransitionParams @params)
        => context.Transition(new BanState(@params.IdentityInfo, @params.Reason));

    public override void ToDraft(ITimeEntryStateContext context, TransitionParams @params)
        => context.Transition(new DraftState(@params.IdentityInfo, @params.Reason));

    public override void ToPreDeleted(ITimeEntryStateContext context, TransitionParams @params)
        => context.Transition(new PreDeleteState(@params.IdentityInfo, @params.Reason));
}
