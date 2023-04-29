#nullable enable

namespace Itixo.Timesheets.Shared.Messaging;

public class TogglSyncMessage
{
    public string Message { get; set; } = "";
    public State SyncState { get; set; }
    public string? UserIdentifier { get; set; }

    public static TogglSyncMessage CreateFinishedMessage(string? userIdentifier)
    {
        return new TogglSyncMessage {SyncState = State.Finished, Message = "Synchronizace dokončena.", UserIdentifier = userIdentifier };
    }

    public static TogglSyncMessage CreateProgressMessage(string? userIdentifier)
    {
        return new TogglSyncMessage {SyncState = State.InProgress, Message = "Synchronizace probíhá.", UserIdentifier = userIdentifier };
    }

    public static TogglSyncMessage CreateStartedMessage(string? userIdentifier)
    {
        return new TogglSyncMessage {SyncState = State.Started, Message = "Synchronizace spuštěna.", UserIdentifier = userIdentifier};
    }

    public static TogglSyncMessage CreateErroredMessage(string? userIdentifier)
    {
        return new TogglSyncMessage { SyncState = State.Failed, Message = "Při synchronizaci došlo k chybě. Kontaktujte administrátora.", UserIdentifier = userIdentifier };
    }

    public enum State
    {
        Failed,
        Started,
        InProgress,
        Finished
    }
}
