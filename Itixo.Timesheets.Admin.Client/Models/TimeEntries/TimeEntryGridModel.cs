using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Shared.Services;

namespace Itixo.Timesheets.Admin.Client.Models.TimeEntries;

public class TimeEntryGridModel : FilteredQueryTimeEntryItemContractBase
{
    public string GroupId { get; set; } = "";
    public int Id { get; set; }
    public long? ExternalId { get; set; }
    public bool IsGroupItem => ExternalId == null;
    public bool IsChecked { get; set; }
    public List<string> SubItemsExternalIds => GroupId?.Split("#").ToList() ?? new List<string>();
    public bool IsGroupsSubItem { get; set; }
    public override string State => ResolveState();
    public string DisplayLastModifiedDate => LastModifiedDate.ToString(CetDateConversionHelper.DisplayFormat, CetDateConversionHelper.CzechCultureInfo);
    public string DisplayStartTime => StartTime.ToString(CetDateConversionHelper.DisplayFormat, CetDateConversionHelper.CzechCultureInfo);
    public string DisplayStopTime => StopTime.ToString(CetDateConversionHelper.DisplayFormat, CetDateConversionHelper.CzechCultureInfo);

    private string ResolveState()
    {
        if (IsGroupItem)
        {
            return "";
        }

        if (IsApproved)
        {
            return ApprovedState;
        }

        if (IsBan)
        {
            return BanState;
        }

        if (IsDraft)
        {
            return DraftState;
        }

        if (IsPreDelete)
        {
            return PreDeleteState;
        }

        return "";
    }
}
