using System.Collections.Generic;

namespace Itixo.Timesheets.Shared.Extensions;

public static class ListOfStringsExtensions
{
    public static string JoinToString(this IEnumerable<string> values)
    {
        if (values == null)
        {
            return "";
        }

        return string.Join(",", values);
    }
}
