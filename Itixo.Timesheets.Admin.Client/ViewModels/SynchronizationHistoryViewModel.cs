using DotVVM.BusinessPack.Controls;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Hosting;
using Itixo.Timesheets.Admin.Client.ApiServices;
using Itixo.Timesheets.Admin.Client.Models.Configurations;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.SyncHistory;
using Itixo.Timesheets.Shared.Extensions;

namespace Itixo.Timesheets.Admin.Client.ViewModels;

public class SynchronizationHistoryViewModel : MasterPageViewModel
{
    private readonly ISynchronizationHistoryApiService synchronizationHistoryApiService;

    public DateTime FromDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); 

    public DateTime ToDate { get; set; } = new DateTime(
        DateTime.Now.Year,
        DateTime.Now.Month,
        DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

    public BusinessPackDataSet<SyncLogRecordGridItemModel> SyncLogRecordsGridViewDataSet { get; set; }
        = new BusinessPackDataSet<SyncLogRecordGridItemModel> { PagingOptions = new PagingOptions { PageSize = 50 } };

    public SynchronizationHistoryViewModel(IDependencies dependencies, ISynchronizationHistoryApiService synchronizationHistoryApiService) : base(dependencies)
    {
        this.synchronizationHistoryApiService = synchronizationHistoryApiService;
    }

    public override async Task Init()
    {
        await Context.Authorize(new string[] { "TimeEntries.Administrator" });
        await base.Init();
    }

    public override async Task PreRender()
    {
        await base.PreRender();

        if (SyncLogRecordsGridViewDataSet.IsRefreshRequired)
        {
            var gridFilter = new SyncLogRecordsFilter
            {
                FromDate = new DateTimeOffset(FromDate),
                ToDate = new DateTimeOffset(ToDate)
            };

            ApiResult<List<SyncLogRecordGridItemModel>> apiResult = await synchronizationHistoryApiService.GetSyncLogRecordsAsync(gridFilter);

            if (apiResult.Success)
            {
                SyncLogRecordsGridViewDataSet.LoadFromQueryable(apiResult.Value.AsQueryable());
            }
        }
    }

    private IQueryable<SyncLogRecordGridItemModel> CreateDummySource()
    {
        return new List<SyncLogRecordGridItemModel>
        {
            new SyncLogRecordGridItemModel { StartedDate = DateTime.Now, StoppedDate = DateTime.Now.AddMinutes(2), Duration = TimeSpan.FromMinutes(1).ToCustomHoursFormat(), Successful = true},
            new SyncLogRecordGridItemModel { StartedDate = DateTime.Now, StoppedDate = DateTime.Now.AddMinutes(2), Duration = TimeSpan.FromMinutes(1).ToCustomHoursFormat(), Successful = false},
        }.AsQueryable();
    }
}

