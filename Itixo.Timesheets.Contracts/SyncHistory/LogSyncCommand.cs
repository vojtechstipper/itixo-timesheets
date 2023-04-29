using System;
using MediatR;

namespace Itixo.Timesheets.Contracts.SyncHistory;


public class LogSyncCommand : IRequest<LogSyncResult>
{
    public string IdentityExternalId { get; set; }
    public DateTimeOffset StartedDate { get; set; }
    public DateTimeOffset StoppedDate { get; set; }
    public SyncResultKind Kind { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorStackTrace { get; set; }
    public Guid LogSyncRecordId { get; set; }
    public DateTimeOffset SyncedFrom { get; set; }
    public DateTimeOffset SyncedTo { get; set; }

    public enum SyncResultKind
    {
        Start, Success, Error
    }

    public static LogSyncCommand CreateStartedLogSyncCommand(DateTimeOffset startDate, string identityExternalId, DateTimeOffset syncedFrom, DateTimeOffset syncedTo)
    {
        return new LogSyncCommand
        {
            StartedDate = startDate,
            IdentityExternalId = identityExternalId,
            Kind = SyncResultKind.Start,
            SyncedFrom = syncedFrom,
            SyncedTo = syncedTo
        };
    }

    public void ToSuccessState(DateTimeOffset stoppedDate)
    {
        StoppedDate = stoppedDate;
        Kind = SyncResultKind.Success;
    }

    public void ToErrorState(DateTimeOffset stoppedDate, string errorMessage, string errorStackTrace)
    {
        StoppedDate = stoppedDate;
        ErrorMessage = errorMessage;
        ErrorStackTrace = errorStackTrace;
        Kind = SyncResultKind.Error;
    }
}
