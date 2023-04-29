using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Shared.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ViewModels.TimeEntries;

public partial class TimeEntriesViewModel
{
    public void ShowGroupTimeEntries(string groupId)
    {
        if (TimeEntriesFilter.IncludingChildrenGroupIds.Contains(groupId))
        {
            TimeEntriesFilter.IncludingChildrenGroupIds.Remove(groupId);
        }
        else
        {
            TimeEntriesFilter.IncludingChildrenGroupIds.Add(groupId);
        }

        TimeEntriesGridViewDataSet.IsRefreshRequired = true;
    }

    public async Task ApproveTimeEntryDraft(TimeEntryGridModel itemContractItemContract)
    {
        ApiResult<bool> apiResult = await timeEntriesApiService.ApproveTimeEntryDraft(itemContractItemContract);

        DisplayApiResult(apiResult.Validations, Texts.ApproveTimeEntryDraft_SuccessMessage_Record_Was_Approved);

        TimeEntriesGridViewDataSet.IsRefreshRequired = true;
    }

    public async Task BanTimeEntryDraft(TimeEntryGridModel itemContractItemContract)
    {
        ApiResult<TimeEntryGridModel> requestResult = await timeEntriesApiService.BanTimeEntryDraft(itemContractItemContract);

        DisplayApiResult(requestResult.Validations, Texts.BanTimeEntryDraft_SuccessMessage_Record_Was_Banned);

        TimeEntriesGridViewDataSet.IsRefreshRequired = true;
    }

    public async Task ApproveCheckedItems()
    {
        IEnumerable<TimeEntryGridModel> checkedItems = TimeEntriesGridViewDataSet.Items.Where(w => w.IsChecked);

        ApiResult<bool> apiResult = await timeEntriesApiService.ApproveTimeEntryDrafts(checkedItems);

        DisplayApiResult(apiResult.Validations, Texts.BulkApproveTimeEntryDrafts_SuccessMessage_Records_Approvement_Was_Successful);

        TimeEntriesGridViewDataSet.IsRefreshRequired = true;
    }

    public async Task BanCheckedItems()
    {
        var timeEntryDraftGridModels = TimeEntriesGridViewDataSet.Items.Where(w => w.IsChecked).ToList();

        if (!timeEntryDraftGridModels.Any())
        {
            DialogModel.ShowErrorMessageWindow(Texts.BulkBanTimeEntryDrafts_ErrorMessage_Records_Ban_Was_Not_Successful);
            return;
        }

        ApiResult<TimeEntryGridModel> result = await timeEntriesApiService.BanTimeEntryDrafts(timeEntryDraftGridModels);
        DisplayApiResult(result.Validations, Texts.BulkBanTimeEntryDrafts_SuccessMessage_Records_Ban_Was_Successful);
        TimeEntriesGridViewDataSet.IsRefreshRequired = true;
    }
}
