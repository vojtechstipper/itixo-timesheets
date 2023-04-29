using AsyncAwaitBestPractices;
using Itixo.Timesheets.Shared.Messaging;
using MediatR;
using TogglSyncShared;

namespace Itixo.Timesheets.SyncApi.Commands;

public class RunSynchronizationCommandHandler : IRequestHandler<RunSynchronizationCommand, Unit>
{
    private readonly ISynchronizationProcessor synchronizationProcessor;
    private readonly ILogger<RunSynchronizationCommandHandler> logger;

    public RunSynchronizationCommandHandler(ISynchronizationProcessor synchronizationProcessor, ILogger<RunSynchronizationCommandHandler> logger)
    {
        this.synchronizationProcessor = synchronizationProcessor;
        this.logger = logger;
    }

    public async Task<Unit> Handle(RunSynchronizationCommand request, CancellationToken cancellationToken)
    {
        if (request.ManualRun)
        {
            TriggerSyncMessage triggerSyncMessage = new TriggerSyncMessage(request.StartDate, request.EndDate, request.IdentityExternalId);
            synchronizationProcessor.Process(logger, triggerSyncMessage).SafeFireAndForget(x => logger.LogError(x.Message)); 
        }
        else
        {
            synchronizationProcessor.Process(logger).SafeFireAndForget(x => logger.LogError(x.Message));
        }
        return default;
    }
}
