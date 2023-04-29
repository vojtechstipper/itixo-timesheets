using System;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.SyncHistory;
using Refit;
using TogglSyncShared.ApiInterfaces;

namespace TogglSyncShared;

public class SyncLogRecordHandler
{
    private readonly IApiConnectorFactory apiConnectorFactory;

    public LogSyncCommand LogSyncCommand { get; private set; }

    public SyncLogRecordHandler(IApiConnectorFactory apiConnectorFactory)
    {
        this.apiConnectorFactory = apiConnectorFactory;
    }

    public async Task InitializeSyncLogRecord(DateTimeOffset startedDate, string identityExternalId, DateTimeOffset syncFrom, DateTimeOffset syncTo)
    {
        LogSyncCommand = LogSyncCommand.CreateStartedLogSyncCommand(startedDate, identityExternalId, syncFrom, syncTo);
        LogSyncResult logSyncStartResult = await SendRequestLogSync();
        LogSyncCommand.LogSyncRecordId = logSyncStartResult.LogSyncRecordId;
    }

    private async Task<LogSyncResult> SendRequestLogSync()
    {
        ISynchronizationApi synchronizationApi = apiConnectorFactory.CreateApiConnector<ISynchronizationApi>();
        ApiResponse<LogSyncResult> apiResponse = await synchronizationApi.LogSync(LogSyncCommand);
        if (apiResponse.IsSuccessStatusCode)
        {
            return apiResponse.Content;
        }
        else
        {
            throw new AggregateException(apiResponse.Error);
        }
    }

    public async Task LogErroredSynchronization(DateTimeOffset stoppedDate, Exception exception, string stackTrace)
    {
        LogSyncCommand.ToErrorState(stoppedDate, exception.Message, stackTrace);
        await SendRequestLogSync();
    }

    public async Task LogSuccessfulSynchronization(DateTimeOffset stoppedDate)
    {
        LogSyncCommand.ToSuccessState(stoppedDate);
        await SendRequestLogSync();
    }
}
