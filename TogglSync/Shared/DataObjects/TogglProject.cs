using System;

namespace TogglSyncShared.DataObjects;

/// <summary>
/// 
///https://github.com/toggl/toggl_api_docs/blob/master/chapters/projects.md#projects
/// </summary>
public class TogglProject : BaseDataObject
{
    /// <summary>
    /// id: The id of the project
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>
    /// name: The name of the project (string, required, unique for client and workspace)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// wid: workspace ID, where the project will be saved (integer, required)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("wid")]
    public int? WorkspaceId { get; set; }

    /// <summary>
    ///  cid: client ID(integer, not required)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("cid")]
    public int? ClientId { get; set; }

    /// <summary>
    /// active: whether the project is archived or not (boolean, by default true)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("active")]
    public bool? IsActive { get; set; }

    /// <summary>
    /// is_private: whether project is accessible for only project users or for all workspace users (boolean, default true)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("is_private")]
    public bool? IsPrivate { get; set; }

    /// <summary>
    /// template: whether the project can be used as a template (boolean, not required)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("template")]
    public bool? IsTemplateable { get; set; }

    /// <summary>
    /// template_id: id of the template project used on current project's creation
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("template_id")]
    public int? TemplateId { get; set; }

    /// <summary>
    /// billable: whether the project is billable or not (boolean, default true, available only for pro workspaces)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("billable")]
    public bool? IsBillable { get; set; }

    /// <summary>
    /// auto_estimates: whether the esitamated hours is calculated based on task esimations or is fixed manually(boolean, default false, not required, premium functionality)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("auto_estimates")]
    public bool? IsAutoEstimates { get; set; }

    /// <summary>
    /// estimated_hours: if auto_estimates is true then the sum of task estimations is returned, otherwise user inserted hours (integer, not required, premium functionality)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("estimated_hours")]
    public int? EstimatedHours { get; set; }

    /// <summary>
    /// at: timestamp that is sent in the response for PUT, indicates the time task was last updated
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("at")]
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// rate: hourly rate of the project (float, not required, premium functionality)
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("rate")]
    public double? HourlyRate { get; set; }

    public override string ToString()
    {
        return string.Format("Id: {0}, Name: {1}", this.Id, this.Name);
    }
}
