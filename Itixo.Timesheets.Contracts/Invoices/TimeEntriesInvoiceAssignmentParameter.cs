using System.Collections.Generic;

namespace Itixo.Timesheets.Contracts.Invoices;

public class TimeEntriesInvoiceAssignmentParameter
{
    public string Number { get; set; }
    public List<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();

    public class TimeEntry
    {
        public int Id { get; set; }
        public string State { get; set; }
    }
}
