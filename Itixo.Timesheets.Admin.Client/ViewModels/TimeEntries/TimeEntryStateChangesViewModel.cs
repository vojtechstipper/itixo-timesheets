using DotVVM.BusinessPack.Controls;
using Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;
using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Admin.Client.ViewModels.Base;
using Itixo.Timesheets.Client.Shared.ApiServices;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public class TimeEntryStateChangesViewModel : ViewModelBase
{
    private readonly ITimeEntryStateChangesApiService apiService;

    public TimeEntryStateChangesViewModel(ITimeEntryStateChangesApiService apiService)
    {
        this.apiService = apiService;
    }

    public bool IsModalDisplayed { get; set; }

    public BusinessPackDataSet<TimeEntryStateChangeModel> StateChangesDataSet { get; set; }
        = new BusinessPackDataSet<TimeEntryStateChangeModel> { PagingOptions = { PageSize = 10 } };

    public async Task ShowChanges(int timeEntryId)
    {
        IsModalDisplayed = true;

        ApiResult<TimeEntryStateChangesApiService.GetStateChangesResponse> apiResult = await apiService.GetStateChangesAsync(timeEntryId);
        if (apiResult.Success)
        {
            StateChangesDataSet.LoadFromQueryable(apiResult.Value.TimeEntryStateChanges.AsQueryable());
        }
        else
        {
            ProcessValidations(apiResult);
        }

        if (!Context.ModelState.IsValid)
        {
            Context.FailOnInvalidModelState();
        }
    }
}
