using System.Collections.Generic;
using Newtonsoft.Json;

namespace Itixo.Timesheets.Shared.Parsers;

public class SortingDictionaryParser
{
    public static Dictionary<SortExpression, SortDirection> Parse(string value)
    {
        return JsonConvert.DeserializeObject<Dictionary<SortExpression, SortDirection>>(value);
    }
}
