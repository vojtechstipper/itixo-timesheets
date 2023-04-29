using System;
using System.Collections.Generic;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;

namespace Itixo.Timesheets.Contracts.Sync;

public class SyncWithLogParameter
{
    public List<TimeEntryContract> TimeEntryContracts { get; set; } = new List<TimeEntryContract>();
    public Guid SyncLogRecordId { get; set; }
}