using DotVVM.BusinessPack.Controls;
using Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;
using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Admin.Client.ViewModels.Base;
using Itixo.Timesheets.Client.Shared.ApiServices;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public class TimeEntryVersionsViewModel : ViewModelBase
{
    private readonly ITimeEntryVersionsApiService timeEntryVersionsApiService;

    public TimeEntryVersionsViewModel(ITimeEntryVersionsApiService timeEntryVersionsApiService)
    {
        this.timeEntryVersionsApiService = timeEntryVersionsApiService;
    }

    public bool IsModalDisplayed { get; set; }

    public BusinessPackDataSet<TimeEntryVersionModel> VersionsDataSet { get; set; }
        = new BusinessPackDataSet<TimeEntryVersionModel>();

    public async Task ShowVersions(int timeEntryId)
    {
        IsModalDisplayed = true;

        ApiResult<TimeEntryVersionsApiService.GetVersionsResult> versionApiResult
            = await timeEntryVersionsApiService.GetVersions(timeEntryId);

        if (versionApiResult.Success)
        {
            var versions = versionApiResult.Value.TimeEntryVersions.ToList();
            VersionsDataSet.LoadFromQueryable(versions.AsQueryable());
        }
        else
        {
            ProcessValidations(versionApiResult);
        }

        if (!Context.ModelState.IsValid)
        {
            Context.FailOnInvalidModelState();
        }
    }
}
