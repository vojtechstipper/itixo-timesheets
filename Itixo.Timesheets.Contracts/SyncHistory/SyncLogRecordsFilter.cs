using System;

namespace Itixo.Timesheets.Contracts.SyncHistory;

public class SyncLogRecordsFilter
{
    public DateTimeOffset FromDate { get; set; }
    public DateTimeOffset ToDate { get; set; }
}
