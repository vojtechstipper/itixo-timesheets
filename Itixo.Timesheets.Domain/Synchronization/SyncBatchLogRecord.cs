using System;
using System.Collections.Generic;

namespace Itixo.Timesheets.Domain;

public class SyncBatchLogRecord
{
    public int Id { get; set; }
    public SyncLogRecord SyncLogRecord { get; set; }
    public IList<int> InsertedDraftedRecordsIds { get; set; } = new List<int>();
    public IList<int> UpdatedApprovedRecordsIds { get; set; } = new List<int>();
    public IList<int> InsertedApprovedRecordsIds { get; set; } = new List<int>();
    public IList<int> UpdatedDraftedRecordsIds { get; set; } = new List<int>();
    public DateTimeOffset StartedDate { get; set; }
    public DateTimeOffset? StoppedDate { get; set; }

    public static SyncBatchLogRecord CreateStartedLogRecord(DateTimeOffset startedDate, Guid syncLogId)
    {
        return new SyncBatchLogRecord
        {
            StartedDate = startedDate,
            SyncLogRecord = new SyncLogRecord { Id = syncLogId }
        };
    }

    public void StopStartedLog(DateTimeOffset stoppedDate, IList<int> insertedDraftedRecordsIds, IList<int> insertedApprovedRecordsIds, IList<int> updatedDraftedRecordsIds, IList<int> updatedApprovedRecordsIds)
    {
        StoppedDate = stoppedDate;
        InsertedDraftedRecordsIds = insertedDraftedRecordsIds;
        InsertedApprovedRecordsIds = insertedApprovedRecordsIds;
        UpdatedApprovedRecordsIds = updatedApprovedRecordsIds;
        UpdatedDraftedRecordsIds = updatedDraftedRecordsIds;
    }
}
