using System;
using System.Collections.Generic;
using System.Linq;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Shared;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Application.TimeEntries.FilteredQuery.GroupedQuery;

public interface IFilteredTimeEntriesGroupsSorter : IService
{
    IEnumerable<FilteredTimeEntryItemContract> ApplySorts(IEnumerable<FilteredTimeEntryItemContract> groupedTimeEntries, Dictionary<SortExpression, SortDirection> sorts);
}

public class FilteredTimeEntriesGroupsSorter : IFilteredTimeEntriesGroupsSorter
{
    private IEnumerable<FilteredTimeEntryItemContract> groupedTimeEntries;

    public IEnumerable<FilteredTimeEntryItemContract> ApplySorts(IEnumerable<FilteredTimeEntryItemContract> groupedTimeEntries, Dictionary<SortExpression, SortDirection> sorts)
    {
        this.groupedTimeEntries = groupedTimeEntries;

        foreach (KeyValuePair<SortExpression, SortDirection> filterAppliedSorting in sorts)
        {
            switch (filterAppliedSorting.Key)
            {
                case SortExpression.Description:
                    this.groupedTimeEntries = ApplySorting(filterAppliedSorting.Value, x => x.Description);
                    break;
                case SortExpression.StartTime:
                    this.groupedTimeEntries = ApplySorting(filterAppliedSorting.Value, x => x.StartTime);
                    break;
                case SortExpression.StopTime:
                    this.groupedTimeEntries = ApplySorting(filterAppliedSorting.Value, x => x.StopTime);
                    break;
                case SortExpression.Duration:
                    this.groupedTimeEntries = ApplySorting(filterAppliedSorting.Value, x => x.Duration);
                    break;
                case SortExpression.TaskName:
                    this.groupedTimeEntries = ApplySorting(filterAppliedSorting.Value, x => x.TaskName);
                    break;
                case SortExpression.ProjectName:
                    this.groupedTimeEntries = ApplySorting(filterAppliedSorting.Value, x => x.ProjectName);
                    break;
                case SortExpression.State:
                    this.groupedTimeEntries = ApplySorting(filterAppliedSorting.Value, x => x.State);
                    break;
                case SortExpression.Username:
                    this.groupedTimeEntries = ApplySorting(filterAppliedSorting.Value, x => x.Username);
                    break;
                case SortExpression.InvoiceNumber:
                    this.groupedTimeEntries = ApplySorting(filterAppliedSorting.Value, x => x.InvoiceNumber);
                    break;
                case SortExpression.LastModifiedDate:
                    this.groupedTimeEntries = ApplySorting(filterAppliedSorting.Value, x => x.LastModifiedDate);
                    break;
            }
        }

        return this.groupedTimeEntries.ToList();
    }

    private IEnumerable<FilteredTimeEntryItemContract> ApplySorting<TProp>(SortDirection direction, Func<FilteredTimeEntryItemContract, TProp> keySelector)
    {
        return direction == SortDirection.Ascending
            ? groupedTimeEntries.OrderBy(keySelector).ToList()
            : groupedTimeEntries.OrderByDescending(keySelector).ToList();
    }
}
