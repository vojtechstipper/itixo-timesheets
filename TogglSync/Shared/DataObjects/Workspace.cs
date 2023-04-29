using Itixo.Timesheets.Shared.Abstractions;
using System;

namespace TogglSyncShared.DataObjects;

/// <summary>
/// https://github.com/toggl/toggl_api_docs/blob/master/chapters/workspaces.md#workspaces
/// </summary>
public class Workspace : BaseDataObject, ITogglWorkspaceDto
{
    /// <summary>
    /// id: The Workspace id
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>
    /// name: (string, required)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// premium: If it's a pro workspace or not. Shows if someone is paying for the workspace or not (boolean, not required)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("premium")]
    public bool? Ispremium { get; set; }

    /// <summary>
    /// at: timestamp that is sent in the response, indicates the time item was last updated
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("at")]
    public DateTime? UpdatedOn { get; set; }

    public override string ToString()
    {
        return string.Format("Id: {0}, Name: {1}", this.Id, this.Name);
    }
}
