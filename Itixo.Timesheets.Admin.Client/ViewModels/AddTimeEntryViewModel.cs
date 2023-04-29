using DotVVM.Framework.ViewModel.Validation;
using Itixo.Timesheets.Admin.Client.ApiServices;
using Itixo.Timesheets.Admin.Client.Models.AddTimeEntry;
using Itixo.Timesheets.Client.Shared.ApiServices;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public class AddTimeEntryViewModel : MasterPageViewModel
{
    private readonly IAddTimeEntryApiService addTimeEntryApiService;

    public AddTimeEntryFormModel AddTimeEntryForm { get; set; } = new AddTimeEntryFormModel();

    public AddTimeEntryViewModel(IDependencies dependencies, IAddTimeEntryApiService addTimeEntryApiService) : base(dependencies)
    {
        this.addTimeEntryApiService = addTimeEntryApiService;
    }

    public override async Task PreRender()
    {
        await base.PreRender();

        if (!Context.IsPostBack)
        {
            AddTimeEntryForm.Projects = (await addTimeEntryApiService.GetProjectsAsync()).Value;
            AddTimeEntryForm.Accounts = (await addTimeEntryApiService.GetAccountsAsync()).Value;
        }
    }

    public async Task AddTimeEntry()
    {
        if (string.IsNullOrWhiteSpace(AddTimeEntryForm.Description))
        {
            this.AddModelError($"Vyplňte popis {nameof(AddTimeEntryForm.Description)}");
        }

        if (AddTimeEntryForm.ProjectId == 0)
        {
            this.AddModelError($"Vyberte projekt {nameof(AddTimeEntryForm.ProjectId)}");
        }

        if (AddTimeEntryForm.TimeTrackerAccountId == 0)
        {
            this.AddModelError($"Vyberte účet {nameof(AddTimeEntryForm.TimeTrackerAccountId)}");
        }

        if (Context.ModelState.IsValid)
        {
            ApiResult<bool> apiResult = await addTimeEntryApiService.AddTimeEntryAsync(AddTimeEntryForm);
            DisplayApiResult(apiResult.Validations, "Záznam byl úspěšně vytvořen.");

            if (apiResult.Success)
            {
                await Reset();
            }
        }
        else
        {
            Context.FailOnInvalidModelState();
        }
    }

    private async Task Reset()
    {
        AddTimeEntryForm = new AddTimeEntryFormModel();
        AddTimeEntryForm.Projects = (await addTimeEntryApiService.GetProjectsAsync()).Value;
        AddTimeEntryForm.Accounts = (await addTimeEntryApiService.GetAccountsAsync()).Value;
    }
}

