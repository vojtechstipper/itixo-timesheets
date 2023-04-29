using System;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Itixo.Timesheets.Shared.Converters;

public class CetDateTimeOffsetConverter : IsoDateTimeConverter
{
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        object value = base.ReadJson(reader, objectType, existingValue, serializer);
        if (value is DateTimeOffset date)
        {
            bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            return isWindows ? TimeZoneInfo.ConvertTime(date, TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"))
                : TimeZoneInfo.ConvertTime(date, TimeZoneInfo.FindSystemTimeZoneById("Europe/Budapest"));
        }
        return value;
    }
}
