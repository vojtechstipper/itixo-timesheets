using System.Collections.Generic;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Refit;

namespace TogglSyncShared.ApiInterfaces;

public interface ITimeTrackerAccountsApi
{
    [Get("/TimeTrackerAccounts/SyncContractList")]
    Task<List<AccountSyncContract>> Get();
}
