namespace Itixo.Timesheets.Users.Client.ViewModels;

public class UnauthorizedViewModel : MasterPageViewModel
{
    public UnauthorizedViewModel(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
    }
}

