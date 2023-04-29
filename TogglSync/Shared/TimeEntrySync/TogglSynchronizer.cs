using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TogglSyncShared.DataObjects;
using TogglSyncShared.Extensions;
using TogglSyncShared.InvalidRecords;
using Task = System.Threading.Tasks.Task;

namespace TogglSyncShared.TimeEntrySync;

public interface ITogglSynchronizer : IService
{
    Task SynchronizeAsync(Guid logSyncRecordId, DateTime @from, DateTime to, ILogger logger);
}

public class TogglSynchronizer : ITogglSynchronizer
{
    private readonly ITimeEntryBatchSender timeEntryBatchSender;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly ITogglProjectSynchronizer togglProjectSynchronizer;
    private readonly ITogglUsersTimeEntriesProvider togglUsersTimeEntriesProvider;
    private readonly ITogglUsersTimeEntryParamsFactory togglUsersTimeEntryParamsProviderFactory;
    private readonly IAccountsForSynchronizationProvider accountsForSynchronizationProvider;
    private readonly IInvalidTogglRercordsReporter invalidRecordsReporter;
    private readonly IConfiguration configuration;

    public TogglSynchronizer(
        ITogglProjectSynchronizer togglProjectSynchronizer,
        ITogglUsersTimeEntriesProvider togglUsersTimeEntriesProvider,
        ITogglUsersTimeEntryParamsFactory togglUsersTimeEntryParamsProviderFactory,
        ITimeEntryBatchSender timeEntryBatchSender,
        IDateTimeProvider dateTimeProvider,
        IAccountsForSynchronizationProvider accountsForSynchronizationProvider,
        IInvalidTogglRercordsReporter invalidRecordsReporter,
        IConfiguration configuration)
    {
        this.togglProjectSynchronizer = togglProjectSynchronizer;
        this.togglUsersTimeEntriesProvider = togglUsersTimeEntriesProvider;
        this.togglUsersTimeEntryParamsProviderFactory = togglUsersTimeEntryParamsProviderFactory;
        this.timeEntryBatchSender = timeEntryBatchSender;
        this.dateTimeProvider = dateTimeProvider;
        this.accountsForSynchronizationProvider = accountsForSynchronizationProvider;
        this.invalidRecordsReporter = invalidRecordsReporter;
        this.configuration = configuration;
    }

    public async Task SynchronizeAsync(Guid logSyncRecordId, DateTime @from, DateTime to, ILogger logger)
    {
        List<AccountSyncContract> accounts = await accountsForSynchronizationProvider.Get();

        foreach (AccountSyncContract account in accounts)
        {
            try
            {
              
                TogglUsersTimeEntryParams togglUsersTimeEntryParams =
                    await togglUsersTimeEntryParamsProviderFactory.CreateAsync(from, to, account.ExternalId);

                // stahnu time entries z Toggl API
                List<TogglTimeEntry> togglUsersTimeEntries = await togglUsersTimeEntriesProvider.GetWhereInWorkspaceIdsAsync(togglUsersTimeEntryParams);

                // synchronizuju projekty, ktere ma ten uzivatel v ramci svych time entries
                await togglProjectSynchronizer.Synchronize(account.ExternalId);

                // kontrola validity zaznamu. vraci kolekci chybovych stavu
                var reportersInvalidRecords = ReportersInvalidRecord.CreateListFrom(togglUsersTimeEntries, togglProjectSynchronizer.SynchronizedProjects);
                await invalidRecordsReporter.ReportInvalidRecords(reportersInvalidRecords, account.Email);

                togglUsersTimeEntries = togglUsersTimeEntries.AssignFictionalProjectWhenNoProject(configuration["FictionalProject:ExternalId"]);

                togglUsersTimeEntries = togglUsersTimeEntries
                    .RemoveInvalidRecords(w => w.IsProjectInvalid())
                    .RemoveInvalidRecords(w => !w.IsTrackingFinished())
                    .RemoveInvalidRecords(w => w.IsDescriptionEmpty());

                var timeEntryContracts = new TogglTimeEntryDataTransformer(dateTimeProvider.Now)
                    .Transform(togglUsersTimeEntries, account)
                    .OrderBy(x => x.StartTime)
                    .ToList();

                if (timeEntryContracts.Any())
                {
                    await timeEntryBatchSender.Send(timeEntryContracts, logSyncRecordId);
                }
            }
            catch (Exception exception)
            {
                string stackTrace = exception.Demystify().StackTrace;

                logger.LogError(exception, exception.Message);
                logger.LogError(stackTrace);
            }
        }
    }
}
