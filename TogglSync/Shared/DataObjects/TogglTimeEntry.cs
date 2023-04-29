using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace TogglSyncShared.DataObjects;

public class TogglTimeEntry : BaseDataObject
{
    [System.Text.Json.Serialization.JsonPropertyName("id")]
    public long? Id { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("description")]
    public string Description { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("wid")]
    public long? WorkspaceId { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("pid")]
    public long? ProjectId { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("tid")]
    public long? TaskId { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("task")]
    public string TaskName { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("billable")]
    public bool? IsBillable { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("start")]
    [JsonConverter(typeof(IsoDateTimeConverter))]
    public DateTimeOffset? Start { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("stop")]
    [JsonConverter(typeof(IsoDateTimeConverter))]
    public DateTimeOffset? Stop { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("duration")]
    public long? Duration { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("created_with")]
    public string CreatedWith { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("tags")]
    public List<string> TagNames { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("duronly")]
    public bool? ShowDurationOnly { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("at")]
    public DateTime? UpdatedOn { get; set; }

    public override string ToString()
    {
        return string.Format("Id: {0}, Start: {1}, Stop: {2}, TaskId: {3}", this.Id, this.Start, this.Stop, this.TaskId);
    }
}
