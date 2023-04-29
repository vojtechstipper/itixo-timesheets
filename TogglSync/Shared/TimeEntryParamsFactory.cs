using System;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.Configurations;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Extensions;
using Itixo.Timesheets.Shared.Services;
using TogglSyncShared.ApiInterfaces;

namespace TogglSyncShared;

public interface ISyncDatesProvider : IService
{
    public Task<(DateTime from, DateTime to)> GetSyncDatesAsync();
}

public class SyncDatesProvider : ISyncDatesProvider
{
    private readonly IApiConnectorFactory apiConnectorFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    public SyncDatesProvider(IApiConnectorFactory apiConnectorFactory, IDateTimeProvider dateTimeProvider)
    {
        this.apiConnectorFactory = apiConnectorFactory;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task<(DateTime from, DateTime to)> GetSyncDatesAsync()
    {
        ISynchronizationApi synchronizationApi = apiConnectorFactory.CreateApiConnector<ISynchronizationApi>();
        SyncBusinessDaysContract syncBusinessDaysContract = await synchronizationApi.Get();

        DateTime from = dateTimeProvider.Now.DateTime.AddBusinessDays(-syncBusinessDaysContract.StartSyncBusinessDaysAgoValue);
        DateTime to = dateTimeProvider.Now.DateTime.AddBusinessDays(-syncBusinessDaysContract.StopSyncBusinessDaysAgoValue);

        return (from, to);
    }
}
