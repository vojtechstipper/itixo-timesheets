using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;
using Itixo.Timesheets.Admin.Client.ApiServices;
using Itixo.Timesheets.Admin.Client.ViewModels.Base;
using Itixo.Timesheets.Client.Shared.AadAuthorization;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Client.Shared.Models;
using Itixo.Timesheets.Contracts.Configurations;
using Itixo.Timesheets.Shared.Resources;
using Itixo.Timesheets.Shared.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Text;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public class MasterPageViewModel : ViewModelBase
{
    private readonly IConfigurationsApiService configurationsApiService;
    private readonly ICurrentIdentityProvider currentUserProvider;

    protected HttpContext httpContext;
    public DialogModel DialogModel { get; set; } = new DialogModel();
    public bool IsSynchronizationLocked { get; set; }
    public string Title { get; set; }
    public string AccountName { get; set; }

    public MasterPageViewModel(IDependencies dependencies)
    {
        httpContext = dependencies.ContextAccessor.HttpContext;
        configurationsApiService = dependencies.ConfigurationsApiService;
        currentUserProvider = dependencies.CurrentUserProvider;
    }

    public override async Task Init()
    {
        await Context.Authorize(new string[] { "TimeEntries.Administrator", "TimeEntries.User" });
        await base.Init();
    }

    public override async Task PreRender()
    {
        if (!Context.IsPostBack)
        {
            ApiResult<SyncSharedLockContract> isSyncLockedApiResult = await configurationsApiService.IsSyncLockedAsync();

            if (isSyncLockedApiResult.Success)
            {
                IsSynchronizationLocked = isSyncLockedApiResult.Value.IsLocked;
            }
            else
            {
                DisplayApiResult(isSyncLockedApiResult.Validations);
            }

            Title = CreateTitle();
            AccountName = currentUserProvider.Username;
        }
    }

    private string CreateTitle()
    {
        if (IsSynchronizationLocked)
        {
            return "Probíhá synchronizace";
        }
        else
        {
            return "Itixo Timesheets Manažer";
        }
    }

    protected void DisplayApiResult(Dictionary<string, string[]> validations, string successMessage = "Hodnoty uloženy.")
    {
        var errorMessage = new StringBuilder();

        foreach (KeyValuePair<string, string[]> validationItem in validations)
        {
            foreach (string message in validationItem.Value)
            {
                errorMessage.Append("<span>").Append(Validations.ResourceManager.GetString(message)).Append("</span><br/>");
            }
        }

        if (validations.Count > 0)
        {
            DialogModel.ShowErrorMessageWindow(errorMessage.ToString());
        }
        else
        {
            DialogModel.ShowSuccessMessageWindow(successMessage);
        }
    }

    public async Task Logout()
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

        Context.RedirectToUrlPermanent("/signout-oidc");
    }

    public interface IDependencies
    {
        ICurrentIdentityProvider CurrentUserProvider { get; }
        IHttpContextAccessor ContextAccessor { get; }
        IChromeAuthorizationRequestErrorHandler ChromeAuthorizationRequestErrorHandler { get; }
        IConfigurationsApiService ConfigurationsApiService { get; }
    }

    public class Dependencies : IDependencies
    {
        public Dependencies(IHttpContextAccessor contextAccessor, IChromeAuthorizationRequestErrorHandler chromeAuthorizationRequestErrorHandler, IConfigurationsApiService configurationsApiService, ICurrentIdentityProvider currentUserProvider)
        {
            ContextAccessor = contextAccessor;
            ChromeAuthorizationRequestErrorHandler = chromeAuthorizationRequestErrorHandler;
            ConfigurationsApiService = configurationsApiService;
            CurrentUserProvider = currentUserProvider;
        }

        public ICurrentIdentityProvider CurrentUserProvider { get; }
        public IHttpContextAccessor ContextAccessor { get; }
        public IChromeAuthorizationRequestErrorHandler ChromeAuthorizationRequestErrorHandler { get; }
        public IConfigurationsApiService ConfigurationsApiService { get; }
    }
}
