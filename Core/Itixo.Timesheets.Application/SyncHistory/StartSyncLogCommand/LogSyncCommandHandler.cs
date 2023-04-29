using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.SyncHistory;
using Itixo.Timesheets.Domain;
using MediatR;

namespace Itixo.Timesheets.Application.SyncHistory.StartSyncLogCommand;

public class LogSyncCommandHandler : IRequestHandler<LogSyncCommand, LogSyncResult>
{
    private readonly ISynchronizationHistoryRepository synchronizationHistoryRepository;

    public LogSyncCommandHandler(ISynchronizationHistoryRepository synchronizationHistoryRepository)
    {
        this.synchronizationHistoryRepository = synchronizationHistoryRepository;
    }

    public async Task<LogSyncResult> Handle(LogSyncCommand request, CancellationToken cancellationToken)
    {
        switch (request.Kind)
        {
            default:
            case LogSyncCommand.SyncResultKind.Start:
            {
                var record = SyncLogRecord.CreateStartedLogRecord(request.IdentityExternalId, request.StartedDate, request.SyncedFrom, request.SyncedTo);
                await synchronizationHistoryRepository.AddAsync(record);
                return new LogSyncResult { LogSyncRecordId = record.Id };
            }
            case LogSyncCommand.SyncResultKind.Success:
            {
                SyncLogRecord record = await synchronizationHistoryRepository.GetAsync(request.LogSyncRecordId);
                record.ToSuccessLogRecord(request.StoppedDate);
                await synchronizationHistoryRepository.UpdateAsync(record);
                return new LogSyncResult { LogSyncRecordId = record.Id };
            }
            case LogSyncCommand.SyncResultKind.Error:
            {
                SyncLogRecord record = await synchronizationHistoryRepository.GetAsync(request.LogSyncRecordId);
                record.ToErrorLogRecord(request.StoppedDate, request.ErrorMessage, request.ErrorStackTrace);
                await synchronizationHistoryRepository.UpdateAsync(record);
                return new LogSyncResult { LogSyncRecordId = record.Id };
            }
        }

    }
}
