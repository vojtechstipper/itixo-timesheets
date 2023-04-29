using System;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Extensions;

namespace Itixo.Timesheets.Domain.TimeEntries.States;

public class TimeEntryStateChange : IEntity<int>
{
    public int Id { get; set; }
    public TimeEntryState PreviousState { get; private set; }
    public TimeEntryState NewState { get; private set; }
    public IdentityInfo ChangedByIdentity { get; private set; }
    public string Reason { get; set; }
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    public static TimeEntryStateChange Create(
        TimeEntryState fromState,
        TimeEntryState toState,
        IdentityInfo identityInfo,
        TimeEntryStateChangeReasons reason)
    {
        return Create(fromState, toState, identityInfo, reason.GetEnumDescription());
    }

    public static TimeEntryStateChange Create(
        TimeEntryState fromState,
        TimeEntryState toState,
        IdentityInfo identityInfo,
        string reason)
    {
        return new TimeEntryStateChange
        {
            PreviousState = fromState,
            NewState = toState,
            ChangedByIdentity = identityInfo,
            Reason = reason
        };
    }
}
