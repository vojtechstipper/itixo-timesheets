using DotVVM.BusinessPack.Controls;
using Itixo.Timesheets.Admin.Client.ApiServices.TimeEntries;
using Itixo.Timesheets.Client.Shared.ApiServices;
using Itixo.Timesheets.Contracts.TimeEntries;

namespace Itixo.Timesheets.Admin.Client.ViewModels.Shared;

public interface IPager
{
    Pagingination Paging { get; set; }
    Task RecalculatePaging(TimeEntriesFilter TimeEntriesFilter, IBusinessPackDataSet gridView);
    void GoNextPage(IBusinessPackDataSet gridView);
    void GoPreviousPage(IBusinessPackDataSet gridView);
    void GoFirstPage(IBusinessPackDataSet gridView);
    void GoLastPage(IBusinessPackDataSet gridView);
}

public class Pager : IPager
{
    private readonly ITimeEntriesApiService timeEntriesApiService;

    public Pager(ITimeEntriesApiService timeEntriesApiService)
    {
        this.timeEntriesApiService = timeEntriesApiService;
    }

    public Pagingination Paging { get; set; } = new Pagingination();

    public async Task RecalculatePaging(TimeEntriesFilter timeEntriesFilter, IBusinessPackDataSet gridView)
    {
        ApiResult<TimeEntriesGridPageInfoContract> timeEntriesCountResult
            = await timeEntriesApiService.GetFilteredTimeEntriesPageCountAsync(timeEntriesFilter);

        if (!timeEntriesCountResult.Success)
        {
            return;
        }

        int lastPage = (timeEntriesCountResult.Value.RecordsCount / gridView.PagingOptions.PageSize) + 1;

        if (lastPage != Paging.LastPage)
        {
            Paging = Paging = new Pagingination();
        }

        Paging.LastPage = lastPage;
    }

    public void GoNextPage(IBusinessPackDataSet gridView)
    {
        if (Paging.CurrentPage < Paging.LastPage)
        {
            Paging.GoNext();
            gridView.RequestRefresh();
        }
    }

    public void GoPreviousPage(IBusinessPackDataSet gridView)
    {
        if (Paging.CurrentPage > 1)
        {
            Paging.GoPrevious();
            gridView.RequestRefresh();
        }
    }

    public void GoFirstPage(IBusinessPackDataSet gridView)
    {
        Paging = new Pagingination();
        gridView.RequestRefresh();
    }

    public void GoLastPage(IBusinessPackDataSet gridView)
    {
        if (Paging.CurrentPage < Paging.LastPage)
        {
            Paging.GoLastPage();
            gridView.RequestRefresh();
        }
    }


}
public class Pagingination
{
    public int PageBefore { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageAfter { get; set; } = 2;
    public int LastPage { get; set; }

    public void GoNext()
    {
        if (CurrentPage < PageAfter)
        {
            CurrentPage = PageAfter;
            PageBefore++;
            PageAfter++;
        }
    }

    public void GoPrevious()
    {
        if (CurrentPage > PageBefore)
        {
            CurrentPage = PageBefore;
            PageBefore--;
            PageAfter--;
        }
    }

    public void GoLastPage()
    {
        if (CurrentPage < LastPage)
        {
            CurrentPage = LastPage;
            PageBefore = LastPage - 1;
            PageAfter = LastPage + 1;
        }
    }
}
