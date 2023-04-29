using Itixo.Timesheets.Admin.Client.ApiServices;
using Itixo.Timesheets.Admin.Client.Models;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public class AccountViewModel : MasterPageViewModel
{
    private readonly IAccountApiService accountApiService;

    public IdentityInfoModel IdentityInfo { get; set; }

    public AccountViewModel(IDependencies dependencies, IAccountApiService accountApiService) : base(dependencies)
    {
        this.accountApiService = accountApiService;
    }

    public override async Task PreRender()
    {
        await base.PreRender();

        if (!Context.IsPostBack)
        {
            await LoadCurrentIdentityInfo();
        }
    }

    private async Task LoadCurrentIdentityInfo()
    {
        ApiResult<IdentityInfoModel> apiResult = await accountApiService.GetCurrentIdentityInfo();

        if (apiResult.Success)
        {
            IdentityInfo = apiResult.Value;
        }
        else
        {
            DialogModel.ShowErrorMessageWindow(Texts.Account_Failed_To_Load_Identity_Info);
        }
    }

    public async Task UpdateIdentity()
    {
        ApiResult apiResult = await accountApiService.UpdateCurrentIdentityInfo(IdentityInfo);
        DisplayApiResult(apiResult.Validations);
        await LoadCurrentIdentityInfo();
    }
}

