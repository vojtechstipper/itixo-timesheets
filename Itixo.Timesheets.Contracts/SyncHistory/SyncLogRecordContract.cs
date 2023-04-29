using System;
using System.Collections.Generic;

namespace Itixo.Timesheets.Contracts.SyncHistory;

public class SyncLogRecordContract
{
    public string IdentityName { get; set; }
    public DateTimeOffset StartedDate { get; set; }
    public DateTimeOffset StoppedDate { get; set; }
    public DateTimeOffset SyncedFrom { get; set; }
    public DateTimeOffset SyncedTo { get; set; }
    public string Duration { get; set; }
    public IList<int> InsertedApprovedRecordsIds { get; set; } = new List<int>();
    public IList<int> UpdatedApprovedRecordsIds { get; set; } = new List<int>();
    public IList<int> InsertedDraftedRecordsIds { get; set; } = new List<int>();
    public IList<int> UpdatedDraftedRecordsIds { get; set; } = new List<int>();
    public bool Successful { get; set; }
}
