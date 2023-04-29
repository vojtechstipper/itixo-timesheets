using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Shared.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Admin.Client.ViewModels.TimeEntries;

public partial class PreDeleteTimeEntriesViewModel
{

    public async Task ApproveTimeEntryDraft(TimeEntryGridModel itemContractItemContract)
    {
        ApiResult<bool> apiResult = await timeEntriesApiService.ApproveTimeEntryDraft(itemContractItemContract);

        DisplayApiResult(apiResult.Validations, Texts.ApproveTimeEntryDraft_SuccessMessage_Record_Was_Approved);

        TimeEntriesGridViewDataSet.IsRefreshRequired = true;
    }

    public async Task DeleteTimeEntry(TimeEntryGridModel itemContractItemContract)
    {
        ApiResult<TimeEntryGridModel> requestResult = await preDeletedTimeEntryApiService.DeleteTimeEntry(itemContractItemContract);

        DisplayApiResult(requestResult.Validations, Texts.DeleteTimeEntryDraft_SuccessMessage_Record_Was_Deleted);

        TimeEntriesGridViewDataSet.IsRefreshRequired = true;
    }

    public async Task ApproveCheckedItems()
    {
        IEnumerable<TimeEntryGridModel> checkedItems = TimeEntriesGridViewDataSet.Items.Where(w => w.IsChecked);

        ApiResult<bool> apiResult = await timeEntriesApiService.ApproveTimeEntryDrafts(checkedItems);

        DisplayApiResult(apiResult.Validations, Texts.BulkApproveTimeEntryDrafts_SuccessMessage_Records_Approvement_Was_Successful);

        TimeEntriesGridViewDataSet.IsRefreshRequired = true;
    }

    public async Task DeleteCheckedItems()
    {
        var timeEntryGridModel = TimeEntriesGridViewDataSet.Items.Where(item => item.IsChecked).ToList();
        if (!timeEntryGridModel.Any())
        {
            DialogModel.ShowErrorMessageWindow(Texts.BulkDeleteTimeEntries_ErrorMessage_Records_Not_Selected);
            return;
        }

        ApiResult<TimeEntryGridModel> result = await preDeletedTimeEntryApiService.DeleteTimeEntryPreDeleted(timeEntryGridModel);
        DisplayApiResult(result.Validations, Texts.BulkDeleteTimeEntryDrafts_SuccessMessage_Records_Delete_Was_Successful);
        TimeEntriesGridViewDataSet.IsRefreshRequired = true;
    }
}
