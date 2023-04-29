using Itixo.Timesheets.Admin.Client.Hubs;
using Itixo.Timesheets.Shared.Messaging;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.Amqp.Framing;
using SlimMessageBus;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.Messaging;

public class TogglSyncMessageConsumer : IConsumer<TogglSyncMessage>
{
    private readonly IHubContext<TogglSyncHub, ITogglSyncHub> togglSyncHub;

    public TogglSyncMessageConsumer(IHubContext<TogglSyncHub, ITogglSyncHub> togglSyncHub)
    {
        this.togglSyncHub = togglSyncHub;
    }

    public Task OnHandle(TogglSyncMessage message)
    {
        if (message.SyncState == TogglSyncMessage.State.Finished)
        {
            return togglSyncHub.Clients.Group(message.UserIdentifier).TogglSyncFinished(message.Message);
        }

        if (message.SyncState == TogglSyncMessage.State.Started)
        {
            return togglSyncHub.Clients.Group(message.UserIdentifier).TogglSyncStart(message.Message);
        }

        if (message.SyncState == TogglSyncMessage.State.Failed)
        {
            return togglSyncHub.Clients.Group(message.UserIdentifier).TogglSyncError(message.Message);
        }

        return Task.CompletedTask;
    }
}
