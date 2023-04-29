namespace Itixo.Timesheets.Shared.Extensions;

public static class BooleanExtensions
{
    public static string ToYesOrNoString(this bool value)
    {
        return value ? "Ano" : "Ne";
    }
}