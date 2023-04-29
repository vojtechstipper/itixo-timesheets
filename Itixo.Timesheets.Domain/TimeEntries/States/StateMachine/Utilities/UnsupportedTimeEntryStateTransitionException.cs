using System;

namespace Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;

public class UnsupportedTimeEntryStateTransitionException : Exception
{
    public UnsupportedTimeEntryStateTransitionException() : base("Unsupported TimeEntryState Transition") { }
}
