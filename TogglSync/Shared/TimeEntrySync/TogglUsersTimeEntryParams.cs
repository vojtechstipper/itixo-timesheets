using System.Collections.Generic;
using TogglAPI.NetStandard.QueryObjects;

namespace TogglSyncShared.TimeEntrySync;

public class TogglUsersTimeEntryParams : TimeEntryParams
{
    public string ApiToken { get; set; } = "";
    public IEnumerable<long?> WorkspaceIds { get; set; } = new List<long?>();
}
