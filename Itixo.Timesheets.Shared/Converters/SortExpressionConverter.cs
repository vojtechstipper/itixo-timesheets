using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Itixo.Timesheets.Shared.Parsers;
using Newtonsoft.Json;

namespace Itixo.Timesheets.Shared.Converters;

public class SortExpressionConverter : System.Text.Json.Serialization.JsonConverter<Dictionary<SortExpression, SortDirection>>
{
    public override Dictionary<SortExpression, SortDirection> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        string value = jsonDoc.RootElement.GetRawText();
        return SortingDictionaryParser.Parse(value);
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<SortExpression, SortDirection> value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(JsonConvert.SerializeObject(value.ToArray(), Formatting.Indented));
    }
}
