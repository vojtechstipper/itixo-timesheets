using System.ComponentModel;

namespace Itixo.Timesheets.Domain.TimeEntries.States;

public enum TimeEntryStateChangeReasons
{
    [Description("")]
    Ok,
    [Description("Záznam změněn dva pracovní dny po začátku měření.")]
    ModifiedLately,
    [Description("Čas začátku měření je větší než čas poslední změny.")]
    StartTimeInFuture,
    [Description("Vytvořen manuálně v aplikaci.")]
    CreatedManuallyInApplication,
    [Description("Záznam chyběl v časovém rozmezí příchozí kolekce synchronizace.")]
    MissingInBatch,
    [Description("Záznam přiřazen k fiktivnímu projektu")]
    FictionalProject
}
