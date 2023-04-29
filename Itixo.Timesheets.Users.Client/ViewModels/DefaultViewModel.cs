using Itixo.Timesheets.Client.Shared.AadAuthorization;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Shared.Resources;
using Itixo.Timesheets.Users.Client.Configuration;
using Itixo.Timesheets.Users.Client.Models;
using Itixo.Timesheets.Users.Client.Services;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using AddOrUpdateTimeTrackerAccountResult = Itixo.Timesheets.Contracts.TimeTrackerAccounts.AddOrUpdateTimeTrackerAccountResult;

namespace Itixo.Timesheets.Users.Client.ViewModels;

public class DefaultViewModel : MasterPageViewModel
{
    private readonly IAccountApiService accountApiService;
    private readonly IChromeAuthorizationRequestErrorHandler chromeAuthorizationRequestErrorHandler;
    public AccountFormModel AccountFormModel { get; set; } = new AccountFormModel();

    public DefaultViewModel(
        IAccountApiService accountApiService,
        IHttpContextAccessor httpContextAccessor,
        IChromeAuthorizationRequestErrorHandler chromeAuthorizationRequestErrorHandler) : base(httpContextAccessor)
    {
        this.accountApiService = accountApiService;
        this.chromeAuthorizationRequestErrorHandler = chromeAuthorizationRequestErrorHandler;
    }

    public override async Task PreRender()
    {
        string username = Context.HttpContext.User.Claims.First(f => f.Type == "preferred_username").Value;

        await chromeAuthorizationRequestErrorHandler
            .BootstrapApiRequestWithAuthenticateErrorHandling(
                () => LoadAccount(username),
                () => Context.RedirectToRoutePermanent(RouteNames.Default));
    }

    public async Task LoadAccount(string username)
    {
        ApiResult<AccountFormModel> apiResult = await accountApiService.GetAccountAsync<AccountFormModel>(username);

        if (apiResult.Success)
        {
            AccountFormModel = apiResult.Value;
        }
        else
        {
            AccountFormModel.Username = username;
        }
    }

    public async Task AddOrUpdateUserAsync()
    {
        if (string.IsNullOrWhiteSpace(AccountFormModel.Username))
        {
            DialogModel.ShowErrorMessageWindow(Validations.UserForm_ValidationMessage_Username_Cant_Be_Empty);
            return;
        }

        if (string.IsNullOrWhiteSpace(AccountFormModel.ExternalId))
        {
            DialogModel.ShowErrorMessageWindow(Validations.UserForm_ValidationMessage_Api_Token_Cant_Be_Empty);
            return;
        }

        ApiResult<AccountApiService.TimeTrackerByTypeResponse> togglTimeTrackerApiResult = await accountApiService.GetByTogglTypeAsync();

        if (!togglTimeTrackerApiResult.Success)
        {
            DialogModel.ShowErrorMessageWindow("Nepodařilo se načíst Id Toggl Měřiče");
            Context.FailOnInvalidModelState();
            return;
        }

        AccountFormModel.TimeTrackerId = togglTimeTrackerApiResult.Value.TimeTracker.Id;

        ApiResult<AddOrUpdateTimeTrackerAccountResult> apiResult = await accountApiService.AddOrUpdateAccountAsync(AccountFormModel);
        AccountFormModel.Id = apiResult.Value.Id;
        DisplayApiResult(apiResult.Validations, "Api token úspěšně uložen.");
    }
}
