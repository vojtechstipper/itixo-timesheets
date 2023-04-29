using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using Itixo.Timesheets.Client.Shared.Models;
using Itixo.Timesheets.Shared.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Users.Client.ViewModels;

[Authorize]
public class MasterPageViewModel : DotvvmViewModelBase
{
    private readonly HttpContext httpContext;
    public DialogModel DialogModel { get; set; } = new DialogModel();

    public MasterPageViewModel(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor.HttpContext;
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
}
