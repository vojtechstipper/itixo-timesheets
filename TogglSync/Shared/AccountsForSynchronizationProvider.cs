using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Enums;
using TogglSyncShared.ApiInterfaces;

namespace TogglSyncShared;

public interface IAccountsForSynchronizationProvider : IService
{
    Task<List<AccountSyncContract>> Get();
}

public class AccountsForSynchronizationProvider : IAccountsForSynchronizationProvider
{
    private readonly IApiConnectorFactory apiConnectorFactory;

    public AccountsForSynchronizationProvider(IApiConnectorFactory apiConnectorFactory)
    {
        this.apiConnectorFactory = apiConnectorFactory;
    }

    public async Task<List<AccountSyncContract>> Get()
    {
        ITimeTrackerAccountsApi timeTrackerAccountsApi = apiConnectorFactory.CreateApiConnector<ITimeTrackerAccountsApi>();
        return (await timeTrackerAccountsApi.Get()).Where(w => w.TimeTrackerType == TimeTrackerType.Toggl).ToList();

    }
}
