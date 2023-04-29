using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.Workspaces;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Extensions;
using TogglSyncShared.ApiInterfaces;

namespace TogglSyncShared.TimeEntrySync;

public interface ITogglUsersTimeEntryParamsFactory : IService
{
    Task<TogglUsersTimeEntryParams> CreateAsync(DateTime from, DateTime to, string apiToken);
}

public class TogglUsersTimeEntryParamsFactory : ITogglUsersTimeEntryParamsFactory
{
    private readonly IApiConnectorFactory apiConnectorFactory;

    public TogglUsersTimeEntryParamsFactory(IApiConnectorFactory apiConnectorFactory)
    {
        this.apiConnectorFactory = apiConnectorFactory;
    }

    public async Task<TogglUsersTimeEntryParams> CreateAsync(DateTime from, DateTime to, string apiToken)
    {
        IWorkspaceApi workspaceApi = apiConnectorFactory.CreateApiConnector<IWorkspaceApi>();
        IEnumerable<WorkspaceListContract> workspaces = await workspaceApi.GetAll();

        return new TogglUsersTimeEntryParams
        {
            WorkspaceIds = workspaces.Select(s => (long?)s.ExternalId),
            ApiToken = apiToken,
            StartDate = from.GetDateWithMinimumTime(),
            EndDate = to.GetDateWithMaximumTime()
        };
    }
}
