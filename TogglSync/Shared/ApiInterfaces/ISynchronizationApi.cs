using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.Configurations;
using Itixo.Timesheets.Contracts.Sync;
using Itixo.Timesheets.Contracts.SyncHistory;
using Refit;
using TogglSyncShared.InvalidRecords;

namespace TogglSyncShared.ApiInterfaces;

public interface ISynchronizationApi
{
    [Get("/Synchronization/GetSyncDateRange")]
    Task<SyncBusinessDaysContract> Get();

    [Post("/Synchronization/LogSync")]
    Task<ApiResponse<LogSyncResult>> LogSync([Body] LogSyncCommand command);

    [Post("/Synchronization/SyncAndLog")]
    Task SynchronizeTimeEntriesAsync([Body] SyncWithLogParameter parameter);

    [Get("/Synchronization/getlock")]
    Task<SyncSharedLockContract> GetCurrent();

    [Post("/Synchronization/lock")]
    Task Lock([Body] SharedLockLockingContract lockingContract);

    [Post("/Synchronization/unlock")]
    Task Unlock([Body] SharedLockUnlockingContract unlockingContract);

    [Get("/Synchronization/GetInvalidReport")]
    Task<GetInvalidTimeEntriesReportsResponse> GetInvalidReport([Body] GetInvalidTimeEntriesReportsParameter parameter);

    [Post("/Synchronization/AddOrUpdateInvalidReport")]
    Task AddOrUpdateInvalidReport(AddOrUpdateInvalidTimeEntriesReportCommand command);
}
