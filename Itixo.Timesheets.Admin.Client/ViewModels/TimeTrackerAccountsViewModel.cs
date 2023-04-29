using DotVVM.BusinessPack.Controls;
using DotVVM.Framework.ViewModel.Validation;
using Itixo.Timesheets.Admin.Client.ApiServices;
using Itixo.Timesheets.Admin.Client.Models.TimeTrackerAccounts;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Contracts.TimeTrackers;
using Itixo.Timesheets.Shared.Enums;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public class TimeTrackerAccountsViewModel : MasterPageViewModel
{
    private readonly ITimeTrackerAccountsApiService timeTrackerAccountsApiService;

    public BusinessPackDataSet<TimeTrackerAccountGridModel> AccountsGridViewDataSet { get; set; } = new BusinessPackDataSet<TimeTrackerAccountGridModel>();
    public TimeTrackerAccountDetailModel TimeTrackerAccountDetail { get; set; } = new TimeTrackerAccountDetailModel();
    public List<TimeTrackerContract> TimeTrackers { get; set; } = new List<TimeTrackerContract>();
    public bool IsTimeTrackerTypeOtherThanThisApplication { get; set; }

    public TimeTrackerAccountsViewModel(ITimeTrackerAccountsApiService timeTrackerAccountsApiService, IDependencies dependencies) : base(dependencies)
    {
        this.timeTrackerAccountsApiService = timeTrackerAccountsApiService;
    }

    public override async Task PreRender()
    {
        await base.PreRender();

        if (!Context.IsPostBack)
        {
            await LoadTimeTrackers();
            TimeTrackerAccountDetail.TimeTrackerContract = TimeTrackers.First(f => f.Type == TimeTrackerType.ThisApplication);
        }

        if (AccountsGridViewDataSet.IsRefreshRequired)
        {
            ApiResult<IEnumerable<TimeTrackerAccountGridModel>> apiResult = await timeTrackerAccountsApiService.GetAccountsAsync<TimeTrackerAccountGridModel>();

            if (apiResult.Success)
            {
                AccountsGridViewDataSet.LoadFromQueryable(apiResult.Value.AsQueryable());
            }
        }

        await base.PreRender();
    }

    private async Task LoadTimeTrackers()
    {
        ApiResult<TimeTrackerAccountsApiService.TimeTrackersResponse> timeTrackersApiResult
            = await timeTrackerAccountsApiService.GetTimeEntryTrackersAsync();

        if (!timeTrackersApiResult.Success)
        {
            DialogModel.ShowErrorMessageWindow("Nepodařilo se načíst měřiče");
        }
        else
        {
            TimeTrackers = timeTrackersApiResult.Value.TimeTrackers;
        }
    }

    public async Task AddOrUpdateNewAccount()
    {
        ValidateTimeTrackerAccountDetail();

        if (!Context.ModelState.IsValid)
        {
            Context.FailOnInvalidModelState();
            return;
        }

        TimeTrackerAccountDetail.Ip = httpContext.Connection.RemoteIpAddress.ToString();

        ApiResult<AddOrUpdateTimeTrackerAccountResult> apiResult
            = await timeTrackerAccountsApiService.AddOrUpdateAccountAsync(TimeTrackerAccountDetail);

        if (!apiResult.Success)
        {
            DialogModel.ShowErrorMessageWindow(Validations.AddingAccount_ValidationMessage_Adding_Was_Not_Successful);
            Context.FailOnInvalidModelState();
        }
        else
        {
            AccountsGridViewDataSet.IsRefreshRequired = true;
            DialogModel.ShowSuccessMessageWindow(Texts.AddingAccess_SuccessMessage_Adding_Was_Successful);
        }
    }

    private void ValidateTimeTrackerAccountDetail()
    {
        if (IsTimeTrackerTypeOtherThanThisApplication && string.IsNullOrWhiteSpace(TimeTrackerAccountDetail.ExternalId))
        {
            this.AddModelError(x => x.TimeTrackerAccountDetail.ExternalId, Validations.UserDetailModel_ValidationMessage_ExternalId_is_Required);
        }

        if (IsTimeTrackerTypeOtherThanThisApplication
            && (TimeTrackerAccountDetail.TimeTrackerContract == null || TimeTrackerAccountDetail.TimeTrackerContract.Id == 0))
        {
            this.AddModelError(x => x.TimeTrackerAccountDetail.TimeTrackerContract, Validations.UserDetailModel_ValidationMessage_TimeTrackerId_is_Required);
        }
    }

    public void SelectAccount(TimeTrackerAccountGridModel gridItem)
    {
        TimeTrackerAccountDetail = TimeTrackerAccountDetailModel.From(gridItem);
    }

    public void ClearForm()
    {
        TimeTrackerAccountDetail = TimeTrackerAccountDetailModel.Empty;
    }

    public async Task DeleteAccount(TimeTrackerAccountGridModel gridItem)
    {
        ApiResult<bool> apiResult = await timeTrackerAccountsApiService.DeleteAccountAsync(gridItem.Id);

        if (!apiResult.Success)
        {
            DialogModel.ShowErrorMessageWindow(Texts.DeleteAccount_ErrorMessage_Delete_Was_Not_Successful);
            Context.FailOnInvalidModelState();
        }
        else
        {
            AccountsGridViewDataSet.IsRefreshRequired = true;
            DialogModel.ShowSuccessMessageWindow(Texts.DeleteAccount_SuccessMessage_Delete_Was_Successful);
        }
    }

    public void Changed()
    {
        IsTimeTrackerTypeOtherThanThisApplication = TimeTrackerAccountDetail.TimeTrackerContract?.Type != TimeTrackerType.ThisApplication;
    }
}
