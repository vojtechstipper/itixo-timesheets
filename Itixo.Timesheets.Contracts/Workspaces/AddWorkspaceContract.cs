using Newtonsoft.Json;

namespace Itixo.Timesheets.Contracts.Workspaces;

public class AddWorkspaceContract
{
    [JsonProperty("workspaceName")]
    public string WorkspaceName { get; set; }

    [JsonProperty("ExternalId")]
    public int ExternalId { get; set; }
}
