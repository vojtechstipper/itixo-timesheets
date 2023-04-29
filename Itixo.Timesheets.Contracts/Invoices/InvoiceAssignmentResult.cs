namespace Itixo.Timesheets.Contracts.Invoices;

public class InvoiceAssignmentResult
{
    public string InvoiceNumber { get; set; }
    public int ApprovedTimeEntriesCount { get; set; }
    public int DraftedTimeEntriesCount { get; set; }
    public int BannedTimeEntriesCount { get; set; }
}
