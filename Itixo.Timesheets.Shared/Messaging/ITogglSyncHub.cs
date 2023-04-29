using System.Threading.Tasks;

namespace Itixo.Timesheets.Shared.Messaging;

public interface ITogglSyncHub
{
    Task TogglSyncFinished(string message);
    Task TogglSyncError(string message);
    Task TogglSyncStart(string message);
}
