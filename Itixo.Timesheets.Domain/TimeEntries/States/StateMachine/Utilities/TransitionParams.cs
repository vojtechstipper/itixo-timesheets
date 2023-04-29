using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Shared.Extensions;

namespace Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;

public class TransitionParams
{
    public IdentityInfo IdentityInfo { get; }
    public string Reason { get; }

    public TransitionParams(IdentityInfo identityInfo, string reason)
    {
        IdentityInfo = identityInfo;
        Reason = reason;
    }

    public static TransitionParams Create(IdentityInfo identityInfo, TimeEntryStateChangeReasons reason)
    {
        return Create(identityInfo, reason.GetEnumDescription());
    }

    public static TransitionParams Create(IdentityInfo identityInfo, string reason)
    {
        return new TransitionParams(identityInfo, reason);
    }
}
