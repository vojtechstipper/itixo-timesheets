using DotVVM.BusinessPack.Controls;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;
using Itixo.Timesheets.Admin.Client.ApiServices;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Contracts.Workspaces;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public class WorkspacesViewModel : MasterPageViewModel
{
    private readonly IWorkspacesApiService workspacesApiService;
    public string ApiToken { get; set; }
    public BusinessPackDataSet<TogglWorkspaceContract> UsersTogglWorkspaces { get; set; } = new BusinessPackDataSet<TogglWorkspaceContract>();
    public BusinessPackDataSet<WorkspaceListContract> Workspaces { get; set; } = new BusinessPackDataSet<WorkspaceListContract>();

    public WorkspacesViewModel(IWorkspacesApiService workspacesApiService, IDependencies dependencies) : base(dependencies)
    {
        this.workspacesApiService = workspacesApiService;
    }

    public override async Task Init()
    {
        await Context.Authorize(new string[] { "TimeEntries.Administrator" });
        await base.Init();
    }

    public override async Task PreRender()
    {
        await base.PreRender();

        if (Workspaces.IsRefreshRequired)
        {
            ApiResult<IEnumerable<WorkspaceListContract>> apiResult = await workspacesApiService.GetWorkspaces();

            if (!apiResult.Success)
            {
                DisplayApiResult(apiResult.Validations);
            }
            else
            {
                Workspaces.LoadFromQueryable(apiResult.Value.AsQueryable());
            }
        }
    }

    public async Task LoadWorkspaces()
    {
        if (string.IsNullOrWhiteSpace(ApiToken))
        {
            DialogModel.ShowErrorMessageWindow(Validations.AccountWorkspaces_ValidationMessage_ExternalId_Cant_Be_Empty);
            return;
        }

        IEnumerable<TogglWorkspaceContract> togglWorkspaces = await workspacesApiService.LoadAccountsWorkspaces(ApiToken);
        UsersTogglWorkspaces.LoadFromQueryable(togglWorkspaces.AsQueryable());
    }

    public async Task AddWorkspace(TogglWorkspaceContract contract)
    {
        if (contract.Exists)
        {
            return;
        }

        var addWorkspaceDto = new AddWorkspaceContract { WorkspaceName = contract.WorkspaceName, ExternalId = contract.ExternalId };

        ApiResult<bool> apiResult = await workspacesApiService.AddWorkspace(addWorkspaceDto);

        DisplayApiResult(apiResult.Validations, Texts.AddWorkspace_SuccessMessage_Workspace_Added);

        Workspaces.IsRefreshRequired = true;
        await LoadWorkspaces();
    }

    public async Task RemoveWorkspace(TogglWorkspaceContract contract)
    {
        if (!contract.Exists)
        {
            return;
        }

        ApiResult<bool> apiResult = await workspacesApiService.RemoveWorkspace(contract.ExternalId);

        DisplayApiResult(apiResult.Validations, Texts.RemvoeWorkspaces_Success_Message_Workspace_Removed);

        Workspaces.IsRefreshRequired = true;
        await LoadWorkspaces();
    }

    public async Task RemoveWorkspace(WorkspaceListContract contract)
    {
        ApiResult<bool> apiResult = await workspacesApiService.RemoveWorkspace(contract.ExternalId);

        DisplayApiResult(apiResult.Validations, Texts.RemvoeWorkspaces_Success_Message_Workspace_Removed);

        Workspaces.IsRefreshRequired = true;

        if (!string.IsNullOrWhiteSpace(ApiToken))
        {
            await LoadWorkspaces();
        }
    }
}

