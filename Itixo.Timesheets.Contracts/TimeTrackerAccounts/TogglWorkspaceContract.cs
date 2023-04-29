using Itixo.Timesheets.Shared.Extensions;

namespace Itixo.Timesheets.Contracts.TimeTrackerAccounts;

public class TogglWorkspaceContract
{
    public int WorkspaceId { get; set; }
    public string WorkspaceName { get; set; }
    public int ExternalId { get; set; }
    public string DisplayExists => Exists.ToYesOrNoString();
    public bool Exists { get; set; }
}
