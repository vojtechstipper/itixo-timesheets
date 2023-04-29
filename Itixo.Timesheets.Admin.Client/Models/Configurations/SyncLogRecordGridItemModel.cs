using System;
using System.Collections.Generic;

namespace Itixo.Timesheets.Admin.Client.Models.Configurations;

public class SyncLogRecordGridItemModel
{
    public string IdentityName { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime StoppedDate { get; set; }
    public DateTime SyncedFrom { get; set; }
    public DateTime SyncedTo { get; set; }
    public string Duration { get; set; }
    public IList<int> InsertedDraftedRecordsIds { get; set; } = new List<int>();
    public IList<int> UpdatedApprovedRecordsIds { get; set; } = new List<int>();
    public IList<int> InsertedApprovedRecordsIds { get; set; } = new List<int>();
    public IList<int> UpdatedDraftedRecordsIds { get; set; } = new List<int>();
    public int InsertedApprovedCount => InsertedApprovedRecordsIds.Count;
    public int UpdatedApprovedCount => UpdatedApprovedRecordsIds.Count;
    public int InsertedDraftedCount => InsertedDraftedRecordsIds.Count;
    public int UpdatedDraftedCount => UpdatedDraftedRecordsIds.Count;
    public bool Successful { get; set; }
}
