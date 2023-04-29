namespace Itixo.Timesheets.Application.TimeEntries.Services;

public class AssignInvoiceResult
{
    public int AssignedTimeEntriesCount { get; set; }

    public AssignInvoiceResult(int timeEntriesCount)
    {
        AssignedTimeEntriesCount = timeEntriesCount;
    }
}
