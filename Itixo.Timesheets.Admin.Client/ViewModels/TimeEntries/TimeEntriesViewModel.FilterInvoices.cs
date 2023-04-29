using System.Collections.Generic;
using System.Linq;
using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Shared.Enums;

namespace Itixo.Timesheets.Admin.Client.ViewModels.TimeEntries;

public partial class TimeEntriesViewModel
{
    public List<InvoiceFilterPair> InvoiceStates { get; set; } = new List<InvoiceFilterPair>
    {
        new InvoiceFilterPair {TimeEntryInvoiceState = TimeEntryInvoiceState.PartlyInvoiced},
        new InvoiceFilterPair {TimeEntryInvoiceState = TimeEntryInvoiceState.OnlyUninvoiced},
        new InvoiceFilterPair {TimeEntryInvoiceState = TimeEntryInvoiceState.OnlyInvoiced},
        new InvoiceFilterPair {TimeEntryInvoiceState = TimeEntryInvoiceState.AllInvoiceStates}
    };

    public List<InvoiceFilterPair> SelectedInvoiceStates { get; set; } = new List<InvoiceFilterPair>();

    public void InvoiceStateSelectionChanged()
    {
        TimeEntriesFilter.SelectedInvoiceStates = new List<TimeEntryInvoiceState>(SelectedInvoiceStates.Select(s => s.TimeEntryInvoiceState));  
    }
}
