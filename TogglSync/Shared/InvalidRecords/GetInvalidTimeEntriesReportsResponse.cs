using Itixo.Timesheets.Contracts.Synchronization;

namespace TogglSyncShared.InvalidRecords;

public class GetInvalidTimeEntriesReportsResponse
{
    public InvalidTimeEntriesReportContract InvalidTimeEntriesReport { get; set; }
        = new InvalidTimeEntriesReportContract();
}
