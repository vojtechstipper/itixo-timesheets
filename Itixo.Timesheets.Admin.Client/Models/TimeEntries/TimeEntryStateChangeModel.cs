using Itixo.Timesheets.Shared.Services;

namespace Itixo.Timesheets.Admin.Client.Models.TimeEntries;

public class TimeEntryStateChangeModel
{
    public DateTimeOffset When { get; set; }
    public string DisplayWhen => When.ToString(CetDateConversionHelper.DisplayFormat, CetDateConversionHelper.CzechCultureInfo);
    public string Who { get; set; }
    public string Why { get; set; }
    public string From { get; set; }
    public string To { get; set; }
}
