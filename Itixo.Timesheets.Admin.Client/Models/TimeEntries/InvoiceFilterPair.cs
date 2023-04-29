using Itixo.Timesheets.Shared.Enums;

namespace Itixo.Timesheets.Admin.Client.Models.TimeEntries;

public class InvoiceFilterPair
{
    public TimeEntryInvoiceState TimeEntryInvoiceState { get; set; }
    public string Description => TimeEntryInvoiceState.GetTimeEntryInvoiceStateDescription();
}
