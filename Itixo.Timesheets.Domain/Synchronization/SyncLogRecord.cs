using System;
using System.Collections.Generic;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Domain;

public class SyncLogRecord : IEntity<Guid>
{
    public Guid Id { get; set; }
    public IdentityInfo IdentityInfo { get; set; }
    public DateTimeOffset StartedDate { get; set; }
    public DateTimeOffset? StoppedDate { get; set; }
    public DateTimeOffset SyncedRecordsFrom { get; set; }
    public DateTimeOffset SyncedRecordsTo { get; set; }
    public string ErrorMessage { get; set; }
    public TimeSpan Duration => (StoppedDate ?? StartedDate) - StartedDate;
    public List<SyncBatchLogRecord> Batches { get; set; } = new List<SyncBatchLogRecord>();

    public static SyncLogRecord CreateStartedLogRecord(string identityExternalId, DateTimeOffset startedDate, DateTimeOffset syncedFrom, DateTimeOffset syncedTo)
    {
        return new SyncLogRecord
        {
            IdentityInfo = new IdentityInfo { ExternalId = identityExternalId },
            StartedDate = startedDate,
            SyncedRecordsFrom = syncedFrom,
            SyncedRecordsTo = syncedTo
        };
    }

    public void ToSuccessLogRecord(DateTimeOffset stoppedDate)
    {
        StoppedDate = stoppedDate;
    }

    public void ToErrorLogRecord(DateTimeOffset stoppedDate, string errorMessage, string stackTrace)
    {
        StoppedDate = stoppedDate;
        ErrorMessage = $"{errorMessage}\n{stackTrace}";
    }
}
