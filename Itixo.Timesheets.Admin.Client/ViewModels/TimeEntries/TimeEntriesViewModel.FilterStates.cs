using System.Collections.Generic;
using Itixo.Timesheets.Contracts.TimeEntries;

namespace Itixo.Timesheets.Admin.Client.ViewModels.TimeEntries;

public partial class TimeEntriesViewModel
{
    public const string AllStates = "Všechny stavy";

    public List<string> States { get; set; } = new List<string>();
    public List<string> SelectedStates { get; set; } = new List<string>();

    private void LoadStates() => States = new List<string>
    {
        FilteredQueryTimeEntryItemContractBase.ApprovedState,
        FilteredQueryTimeEntryItemContractBase.DraftState,
        FilteredQueryTimeEntryItemContractBase.BanState,
        AllStates
    };

    public void StatesSelectionChanged()
    {
        TimeEntriesFilter.IsDraftedRequired = SelectedStates.Contains(FilteredQueryTimeEntryItemContractBase.DraftState);
        TimeEntriesFilter.IsBannedRequired = SelectedStates.Contains(FilteredQueryTimeEntryItemContractBase.BanState);
        TimeEntriesFilter.IsApprovedRequired = SelectedStates.Contains(FilteredQueryTimeEntryItemContractBase.ApprovedState);

        if (SelectedStates.Contains(AllStates))
        {
            TimeEntriesFilter.IsDraftedRequired = false;
            TimeEntriesFilter.IsBannedRequired = false;
            TimeEntriesFilter.IsApprovedRequired = false;
        }
    }
}
