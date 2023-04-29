using System.ComponentModel;

namespace Itixo.Timesheets.Domain.TimeEntries.States;

public enum TimeEntryState
{
    [Description("Žádný")]
    None,
    [Description("Ke schválení")]
    Draft,
    [Description("Zamítnut")]
    Ban,
    [Description("Schválený")]
    Approved,
    [Description("Předsmazán")]
    Predeleted
}
