using DotVVM.BusinessPack.Controls;
using Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;
using Itixo.Timesheets.Admin.Client.Configurations;
using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Admin.Client.ViewModels.Shared;
using Itixo.Timesheets.Admin.Client.ViewModels.Shared.TimeEntries;
using Itixo.Timesheets.Client.Shared.AadAuthorization;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.TimeEntries;

namespace Itixo.Timesheets.Admin.Client.ViewModels.TimeEntries;

public partial class TimeEntriesViewModel : MasterPageViewModel
{
    private readonly ITimeEntriesApiService timeEntriesApiService;
    public IUsersFilterModel UsersFilterModel { get; set; }
    public IProjectsFilterModel ProjectsFilterModel { get; set; }
    public IClientsFilterModel ClientsFilterModel { get; set; }
    public IPager Pager { get; set; }

    private readonly IChromeAuthorizationRequestErrorHandler chromeAuthorizationRequestErrorHandler;

    public BusinessPackDataSet<TimeEntryGridModel> TimeEntriesGridViewDataSet { get; set; } =
        new BusinessPackDataSet<TimeEntryGridModel> { PagingOptions = { PageSize = 50 } };
    public bool IsAnyItemChecked => TimeEntriesGridViewDataSet.Items.Any(item => item.IsChecked);
    public TimeEntryVersionsViewModel TimeEntryVersionsViewModel { get; set; }
    public TimeEntryStateChangesViewModel TimeEntryStateChangesViewModel { get; set; }
    public TimeEntriesFilter TimeEntriesFilter { get; set; } = new TimeEntriesFilter
    {
        FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
        ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
    };

    public TimeEntriesViewModel(
        TimeEntryVersionsViewModel timeEntryVersionsViewModel,
        TimeEntryStateChangesViewModel timeEntryStateChangesViewModel,
        ITimeEntriesApiService timeEntriesApiService,
        IUsersFilterModel usersFilterModel,
        IProjectsFilterModel projectsFilterModel,
        IClientsFilterModel clientsFilterModel,
        IPager pager,
        IDependencies dependencies) : base(dependencies)
    {
        TimeEntryVersionsViewModel = timeEntryVersionsViewModel;
        TimeEntryStateChangesViewModel = timeEntryStateChangesViewModel;
        this.timeEntriesApiService = timeEntriesApiService;
        UsersFilterModel = usersFilterModel;
        ProjectsFilterModel = projectsFilterModel;
        ClientsFilterModel = clientsFilterModel;
        Pager = pager;
        chromeAuthorizationRequestErrorHandler = dependencies.ChromeAuthorizationRequestErrorHandler;
    }

    public override async Task PreRender()
    {
        if (!Context.IsPostBack)
        {
            await chromeAuthorizationRequestErrorHandler.BootstrapApiRequestWithAuthenticateErrorHandling
                (ProjectsFilterModel.LoadProjects, () => Context.RedirectToRoutePermanent(RouteNames.TimeEntries));
            await UsersFilterModel.LoadUsers();
            await ClientsFilterModel.LoadClients();
            LoadStates();
            LoadQueryParamsIntoFilter();
        }

        await base.PreRender();

        if (TimeEntriesGridViewDataSet.IsRefreshRequired)
        {
            await Pager.RecalculatePaging(TimeEntriesFilter, TimeEntriesGridViewDataSet);
            await LoadTimeEntriesIntoGrid();
        }
    }

    private async Task LoadTimeEntriesIntoGrid()
    {
        TimeEntriesFilter.Skip = Pager.Paging.PageBefore * TimeEntriesGridViewDataSet.PagingOptions.PageSize;
        TimeEntriesFilter.Take = TimeEntriesGridViewDataSet.PagingOptions.PageSize;

        ApiResult<IEnumerable<TimeEntryGridModel>> apiResult = await timeEntriesApiService.GetFilteredTimeEntriesAync(TimeEntriesFilter);

        if (apiResult.Success)
        {
            if (!TimeEntriesFilter.IsPreDeleteRequired)
            {
                foreach (TimeEntryGridModel timeEntryGridModel in apiResult.Value.Where(w => !w.IsGroupItem))
                {
                    timeEntryGridModel.IsGroupsSubItem = apiResult.Value.Any(x => x.GroupId != null && x.GroupId.Contains(timeEntryGridModel.ExternalId.ToString()));
                }
            }

            TimeEntriesGridViewDataSet.LoadFromQueryable(apiResult.Value.AsQueryable());
        }
        else
        {
            DisplayApiResult(apiResult.Validations);
        }
    }

    public virtual void CheckOrUncheckItems()
    {
        IEnumerable<TimeEntryGridModel> itemsToCheck = TimeEntriesGridViewDataSet.Items.Where(w => w.State == FilteredQueryTimeEntryItemContractBase.DraftState);

        if (IsAnyItemChecked)
        {
            foreach (TimeEntryGridModel row in itemsToCheck)
            {
                row.IsChecked = false;
            }
        }
        else
        {
            foreach (TimeEntryGridModel row in itemsToCheck)
            {
                row.IsChecked = true;
            }
        }
    }

    public void Filter()
    {
        TimeEntriesFilter.ProjectIds = ProjectsFilterModel.GetSelectedProjectIds();
        TimeEntriesFilter.AccountIds = UsersFilterModel.GetSelectedUserIds();
        TimeEntriesFilter.ClientIds = ClientsFilterModel.GetSelectedClientIds();
        TimeEntriesGridViewDataSet.IsRefreshRequired = true;
    }

    public void RefreshPostback() { }
}
