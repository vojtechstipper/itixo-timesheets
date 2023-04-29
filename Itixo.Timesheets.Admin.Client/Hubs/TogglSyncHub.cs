using Itixo.Timesheets.Shared.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace Itixo.Timesheets.Admin.Client.Hubs;

public class TogglSyncHub : Hub<ITogglSyncHub>
{

    public async Task RegisterUser()
    {
        string groupName = Context.User.Claims.First(f => f.Type == "preferred_username").Value;
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }
}
