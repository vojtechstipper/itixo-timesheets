using Itixo.Timesheets.Contracts.Configurations;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Extensions;
using Itixo.Timesheets.Shared.Messaging;
using Itixo.Timesheets.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SlimMessageBus;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TogglSyncShared.ApiInterfaces;
using TogglSyncShared.TimeEntrySync;

namespace TogglSyncShared;

public interface ISynchronizationProcessor : IService
{
    Task Process(ILogger logger, TriggerSyncMessage message = null);
}

public class SynchronizationProcessor : ISynchronizationProcessor
{
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly IApiConnectorFactory apiConnectorFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    private IMessageBus messageBus;
    private ILogger logger;
    private SyncLogRecordHandler syncLogRecordHandler;
    private string identityExternalId;
    private DateTime syncTo;
    private DateTime syncFrom;

    public SynchronizationProcessor(
        IServiceScopeFactory serviceScopeFactory,
        IApiConnectorFactory apiConnectorFactory,
        IDateTimeProvider dateTimeProvider)
    {
        this.serviceScopeFactory = serviceScopeFactory;
        this.apiConnectorFactory = apiConnectorFactory;
        this.dateTimeProvider = dateTimeProvider;
    }

    public async Task Process(ILogger logger, TriggerSyncMessage message = null)
    {
        try
        {
            this.logger = logger;
            syncLogRecordHandler = new SyncLogRecordHandler(apiConnectorFactory);

            using IServiceScope scope = serviceScopeFactory.CreateScope();
            IConfiguration configuration = scope.ServiceProvider.GetService<IConfiguration>();
            if (message != null)
            {
                identityExternalId = message.IdentityExternalId;
                syncFrom = message.StartDate.GetDateWithMinimumTime();
                syncTo = message.EndDate.GetDateWithMaximumTime();
            }
            else
            {
                identityExternalId = configuration["AzureAd:ObjectId"];
                (syncFrom, syncTo) = await scope.ServiceProvider.GetService<ISyncDatesProvider>().GetSyncDatesAsync();
            }
            ITogglSynchronizer togglSynchronizer = scope.ServiceProvider.GetService<ITogglSynchronizer>();
            messageBus = scope.ServiceProvider.GetService<IMessageBus>();

            await syncLogRecordHandler.InitializeSyncLogRecord(dateTimeProvider.Now, identityExternalId, syncFrom, syncTo);

            await ProcessSynchronization(() => togglSynchronizer.SynchronizeAsync(syncLogRecordHandler.LogSyncCommand.LogSyncRecordId, syncFrom, syncTo, logger));
        }
        catch (Exception exception)
        {
            await OnException(exception, apiConnectorFactory.CreateApiConnector<ISynchronizationApi>());
        }
    }

    private async Task ProcessSynchronization(Func<Task> synchronize)
    {
        var synchronizationApi = apiConnectorFactory.CreateApiConnector<ISynchronizationApi>();

        try
        {
            SyncSharedLockContract synclock = await synchronizationApi.GetCurrent();
            if (synclock?.IsLocked ?? false)
            {
                await messageBus.Publish(TogglSyncMessage.CreateProgressMessage(identityExternalId));
                return;
            }

            await BeforeSync(synchronizationApi);
            await synchronize();
            await AfterSync(synchronizationApi);
        }
        catch (Exception exception)
        {
            await OnException(exception, synchronizationApi);
        }
    }

    private async Task BeforeSync(ISynchronizationApi synchronizationApi)
    {
        await messageBus.Publish(TogglSyncMessage.CreateStartedMessage(identityExternalId));
        await synchronizationApi.Lock(new SharedLockLockingContract { Value = DateTime.Now });
    }

    private async Task AfterSync(ISynchronizationApi synchronizationApi)
    {
        await syncLogRecordHandler.LogSuccessfulSynchronization(dateTimeProvider.Now);
        await synchronizationApi.Unlock(new SharedLockUnlockingContract { Value = DateTime.Now });
        await messageBus.Publish(TogglSyncMessage.CreateFinishedMessage(identityExternalId));
    }

    private async Task OnException(Exception exception, ISynchronizationApi synchronizationApi)
    {
        string stackTrace = exception.Demystify().StackTrace;

        logger.LogError(exception, exception.Message);
        logger.LogError(stackTrace);

        await synchronizationApi.Unlock(new SharedLockUnlockingContract { Value = dateTimeProvider.Now.DateTime });

        await syncLogRecordHandler.LogErroredSynchronization(dateTimeProvider.Now, exception, stackTrace);

        await messageBus.Publish(TogglSyncMessage.CreateErroredMessage(identityExternalId));
    }
}
