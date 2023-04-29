namespace Itixo.Timesheets.Shared.Enums;

public enum TimeEntryInvoiceState
{
    PartlyInvoiced, OnlyInvoiced, OnlyUninvoiced, AllInvoiceStates
}

public static class TimeEntryInvoiceStateExtensions
{
    public const string PartlyInvoiced = "Částečně vyfakturováno";
    public const string OnlyInvoiced = "Jen vyfakturované";
    public const string OnlyUninvoiced = "Jen nevyfakturované";
    public const string AllInvoices = "Všechny";

    public static string GetTimeEntryInvoiceStateDescription(this TimeEntryInvoiceState value)
    {
        return value switch
        {
            TimeEntryInvoiceState.PartlyInvoiced => PartlyInvoiced,
            TimeEntryInvoiceState.OnlyInvoiced => OnlyInvoiced,
            TimeEntryInvoiceState.OnlyUninvoiced => OnlyUninvoiced,
            TimeEntryInvoiceState.AllInvoiceStates => AllInvoices,
            _ => ""
        };
    }
}
