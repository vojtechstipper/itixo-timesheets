using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Shared;
using Itixo.Timesheets.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.TimeEntries;

namespace Itixo.Timesheets.Application.TimeEntries.FilteredQuery;

public interface IFilteredTimeEntriesQuery : IService
{
    Task<IEnumerable<FilteredTimeEntryItemContract>> ExecuteAsync(TimeEntriesFilter filter);
}

public class FilteredTimeEntriesQuery : IFilteredTimeEntriesQuery
{
    private readonly IBaseFilteredTimeEntriesQuery baseFilteredTimeEntriesQuery;

    private TimeEntriesFilter filter;
    private List<FilteredTimeEntryItemContract> results = new List<FilteredTimeEntryItemContract>();

    public FilteredTimeEntriesQuery(IBaseFilteredTimeEntriesQuery baseFilteredTimeEntriesQuery)
    {
        this.baseFilteredTimeEntriesQuery = baseFilteredTimeEntriesQuery;
    }

    public async Task<IEnumerable<FilteredTimeEntryItemContract>> ExecuteAsync(TimeEntriesFilter filter)
    {
        this.filter = filter;

        results = await baseFilteredTimeEntriesQuery.GetFilteredResults<FilteredTimeEntryItemContract>(filter);

        ApplySorts();

        results = results.Skip(filter.Skip).Take(filter.Take).ToList();

        return results;
    }

    private void ApplySorts()
    {
        foreach (KeyValuePair<SortExpression, SortDirection> filterAppliedSorting in filter.AppliedSortings)
        {
            switch (filterAppliedSorting.Key)
            {
                case SortExpression.Description:
                    ApplySorting(filterAppliedSorting.Value, x => x.Description);
                    break;
                case SortExpression.StartTime:
                    ApplySorting(filterAppliedSorting.Value, x => x.StartTime);
                    break;
                case SortExpression.StopTime:
                    ApplySorting(filterAppliedSorting.Value, x => x.StopTime);
                    break;
                case SortExpression.Duration:
                    ApplySorting(filterAppliedSorting.Value, x => x.Duration);
                    break;
                case SortExpression.TaskName:
                    ApplySorting(filterAppliedSorting.Value, x => x.TaskName);
                    break;
                case SortExpression.ProjectName:
                    ApplySorting(filterAppliedSorting.Value, x => x.ProjectName);
                    break;
                case SortExpression.State:
                    ApplySorting(filterAppliedSorting.Value, x => x.State);
                    break;
                case SortExpression.Username:
                    ApplySorting(filterAppliedSorting.Value, x => x.Username);
                    break;
                case SortExpression.InvoiceNumber:
                    ApplySorting(filterAppliedSorting.Value, x => x.InvoiceNumber);
                    break;
                case SortExpression.LastModifiedDate:
                    ApplySorting(filterAppliedSorting.Value, x => x.LastModifiedDate);
                    break;
                case SortExpression.ExternalId:
                    ApplySorting(filterAppliedSorting.Value, x => x.ExternalId);
                    break;
            }
        }
    }

    private void ApplySorting<TProp>(SortDirection direction, Func<FilteredTimeEntryItemContract, TProp> keySelector)
    {
        results =
        direction == SortDirection.Ascending
            ? results.OrderBy(keySelector).ToList()
            : results.OrderByDescending(keySelector).ToList();
    }

}

