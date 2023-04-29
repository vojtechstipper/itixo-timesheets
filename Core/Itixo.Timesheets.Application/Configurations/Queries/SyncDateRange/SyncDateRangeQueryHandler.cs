using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Domain.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Shared.ConstantObjects;

namespace Itixo.Timesheets.Application.Configurations.Queries.SyncDateRange;

public class SyncDateRangeQueryHandler : IRequestHandler<SyncDateRangeQuery, SyncDateRangeQueryResponse>
{
    private readonly IPersistenceQuery<Configuration, int> configurationPersistenceQuery;

    public SyncDateRangeQueryHandler(IPersistenceQuery<Configuration, int> configurationPersistenceQuery)
    {
        this.configurationPersistenceQuery = configurationPersistenceQuery;
    }

    public async Task<SyncDateRangeQueryResponse> Handle(SyncDateRangeQuery request, CancellationToken token)
    {
        string[] configurationKeys = { ConfigurationConstants.StartSyncBusinessDaysAgo, ConfigurationConstants.StopSyncBusinessDaysAgo };
        List<Configuration> configurations = await configurationPersistenceQuery.GetQueryable().Where(w => configurationKeys.Contains(w.Key)).ToListAsync(token);

        Configuration startSyncBusinessDaysAgo = configurations.FirstOrDefault(f => f.Key == ConfigurationConstants.StartSyncBusinessDaysAgo);
        string startSyncBusinessDaysAgoValue = startSyncBusinessDaysAgo?.Value;
        int.TryParse(startSyncBusinessDaysAgoValue, out int startSyncBusinessDaysAgoValueAsInteger);

        Configuration stopSyncBusinessDaysAgo = configurations.FirstOrDefault(f => f.Key == ConfigurationConstants.StopSyncBusinessDaysAgo);
        string stopSyncBusinessDaysAgoValue = stopSyncBusinessDaysAgo?.Value;
        int.TryParse(stopSyncBusinessDaysAgoValue, out int stopSyncBusinessDaysAgoValueAsInteger);

        return new SyncDateRangeQueryResponse
        {
            StartSyncBusinessDaysAgoId = startSyncBusinessDaysAgo?.Id ?? 0,
            StartSyncBusinessDaysAgoValue = startSyncBusinessDaysAgoValueAsInteger,
            StopSyncBusinessDaysAgoId = stopSyncBusinessDaysAgo?.Id ?? 0,
            StopSyncBusinessDaysAgoValue = stopSyncBusinessDaysAgoValueAsInteger
        };
    }
}
