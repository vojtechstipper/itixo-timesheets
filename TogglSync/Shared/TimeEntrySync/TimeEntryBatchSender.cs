using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.Sync;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Shared.Abstractions;
using Microsoft.Extensions.Configuration;
using TogglSyncShared.ApiInterfaces;

namespace TogglSyncShared.TimeEntrySync;

public interface ITimeEntryBatchSender : IService
{
    Task Send(List<TimeEntryContract> timeEntryContracts, Guid syncLogRecordId);
}

public class TimeEntryBatchSender : ITimeEntryBatchSender
{
    private readonly IApiConnectorFactory apiConnectorFactory;
    private readonly int timeEntryItemsInOneDose;

    public TimeEntryBatchSender(IApiConnectorFactory apiConnectorFactory, IConfiguration configuration)
    {
        this.apiConnectorFactory = apiConnectorFactory;
        timeEntryItemsInOneDose = Convert.ToInt32((string?) configuration["TimeEntryItemsInOneDose"]);
    }

    public async Task Send(List<TimeEntryContract> timeEntryContracts, Guid syncLogRecordId)
    {
        ISynchronizationApi synchronizationApi = apiConnectorFactory.CreateApiConnector<ISynchronizationApi>();

        int timeEntryContractsCount = 0;
        while (timeEntryContractsCount < timeEntryContracts.Count)
        {
            var dose = timeEntryContracts.Skip(timeEntryContractsCount).Take(timeEntryItemsInOneDose).ToList();

            if (dose.Any())
            {
                await synchronizationApi.SynchronizeTimeEntriesAsync(new SyncWithLogParameter{ TimeEntryContracts = dose, SyncLogRecordId = syncLogRecordId});
            }

            timeEntryContractsCount += timeEntryItemsInOneDose;
        }
    }
}
