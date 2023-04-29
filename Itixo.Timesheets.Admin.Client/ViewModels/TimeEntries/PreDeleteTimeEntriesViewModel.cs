using DotVVM.BusinessPack.Controls;
using Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;
using Itixo.Timesheets.Admin.Client.Models.TimeEntries;
using Itixo.Timesheets.Admin.Client.ViewModels.Shared;
using Itixo.Timesheets.Admin.Client.ViewModels.Shared.TimeEntries;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.TimeEntries;

namespace Itixo.Timesheets.Admin.Client.ViewModels.TimeEntries;

public partial class PreDeleteTimeEntriesViewModel : MasterPageViewModel
{
    private readonly ITimeEntriesApiService timeEntriesApiService;
    private readonly IPreDeletedTimeEntryApiService preDeletedTimeEntryApiService;

    public IUsersFilterModel UsersFilterModel { get; set; }
    public IProjectsFilterModel ProjectsFilterModel { get; set; }
    public IClientsFilterModel ClientsFilterModel { get; set; }
    public IPager Pager { get; set; }

    public BusinessPackDataSet<TimeEntryGridModel> TimeEntriesGridViewDataSet { get; set; } =
        new BusinessPackDataSet<TimeEntryGridModel> { PagingOptions = { PageSize = 2 } };
    public bool IsAnyItemChecked => TimeEntriesGridViewDataSet.Items.Any(item => item.IsChecked);
    public TimeEntryVersionsViewModel TimeEntryVersionsViewModel { get; set; }
    public TimeEntryStateChangesViewModel TimeEntryStateChangesViewModel { get; set; }
    public TimeEntriesFilter TimeEntriesFilter { get; set; } = new TimeEntriesFilter
    {
        FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
        ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)),
        IsPreDeleteRequired = true
    };
    public PreDeleteTimeEntriesViewModel(
        TimeEntryVersionsViewModel timeEntryVersionsViewModel,
        TimeEntryStateChangesViewModel timeEntryStateChangesViewModel,
        ITimeEntriesApiService timeEntriesApiService,
        IPreDeletedTimeEntryApiService preDeletedTimeEntryApiService,
        IUsersFilterModel usersFilterModel,
        IProjectsFilterModel projectsFilterModel,
        IClientsFilterModel clientsFilterModel,
        IPager pager,
        IDependencies dependencies) : base(dependencies)
    {
        TimeEntryVersionsViewModel = timeEntryVersionsViewModel;
        TimeEntryStateChangesViewModel = timeEntryStateChangesViewModel;
        this.timeEntriesApiService = timeEntriesApiService;
        this.preDeletedTimeEntryApiService = preDeletedTimeEntryApiService;
        this.UsersFilterModel = usersFilterModel;
        this.ProjectsFilterModel = projectsFilterModel;
        this.ClientsFilterModel = clientsFilterModel;
        this.Pager = pager;
    }

    public override async Task PreRender()
    {
        if (!Context.IsPostBack)
        {
            await UsersFilterModel.LoadUsers();
            await ProjectsFilterModel.LoadProjects();
            await ClientsFilterModel.LoadClients();
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

        ApiResult<IEnumerable<TimeEntryGridModel>> apiResult = await preDeletedTimeEntryApiService.GetPreDeleteTimeEntriesAync(TimeEntriesFilter);

        if (apiResult.Success)
        {
            TimeEntriesGridViewDataSet.LoadFromQueryable(apiResult.Value.AsQueryable());
        }
        else
        {
            DisplayApiResult(apiResult.Validations);
        }
    }

    public void CheckOrUncheckItems()
    {
        IEnumerable<TimeEntryGridModel> itemsToCheck = TimeEntriesGridViewDataSet.Items.Where(w => w.State == FilteredQueryTimeEntryItemContractBase.PreDeleteState);

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
