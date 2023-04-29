using DotVVM.BusinessPack.Controls;
using Itixo.Timesheets.Admin.Client.ApiServices;
using Itixo.Timesheets.Admin.Client.Models.Reports;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.TimeEntries;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public partial class ReportsViewModel : MasterPageViewModel
{
    private readonly IReportsApiService reportsApiService;

    public ReportsQueryFilter ReportsQueryFilter { get; set; } = new ReportsQueryFilter();
    public DateTime FromDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
    public DateTime ToDate { get; set; } = new DateTime(
        DateTime.Now.Year,
        DateTime.Now.Month,
        DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

    public BusinessPackDataSet<AccountReportGridItemModel> ReportsGridViewDataSet { get; set; } = new BusinessPackDataSet<AccountReportGridItemModel>()
    {
        PagingOptions = { PageSize = 20 }
    };
    public AccountReportGridSummaryModel AccountReportGridSummaryModel { get; set; }

    public ReportsViewModel(IReportsApiService reportsApiService, IDependencies dependencies) : base(dependencies)
    {
        this.reportsApiService = reportsApiService;
    }

    public override async Task PreRender()
    {
        await base.PreRender();

        if (!Context.IsPostBack)
        {
            await LoadProjects();
            await LoadClients();
        }

        if (ReportsGridViewDataSet.IsRefreshRequired)
        {
            ReportsQueryFilter.FromDate = new DateTimeOffset(FromDate);
            ReportsQueryFilter.ToDate = new DateTimeOffset(ToDate);
            ApiResult<IEnumerable<AccountReportGridItemModel>> gridItemsApiResult = await reportsApiService.GetReportUserGridItemsAsync(ReportsQueryFilter);
            ApiResult<AccountReportGridSummaryModel> summaryResult = await reportsApiService.GetReportUserGridSummaryAsync(ReportsQueryFilter);
            AccountReportGridSummaryModel = summaryResult.Value;

            if (gridItemsApiResult.Success)
            {
                ReportsGridViewDataSet.LoadFromQueryable(gridItemsApiResult.Value.AsQueryable());
            }
        }
    }

    public void Filter()
    {
        ReportsQueryFilter.ProjectIds = GetSelectedProjectIds();
        ReportsQueryFilter.ClientIds = GetSelectedClientIds();
        ReportsGridViewDataSet.IsRefreshRequired = true;
    }

    public void Postback() { }
}
