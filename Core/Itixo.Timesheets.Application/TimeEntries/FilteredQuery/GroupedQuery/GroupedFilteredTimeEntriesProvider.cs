using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Shared.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.TimeEntries.FilteredQuery.GroupedQuery;

public interface IGroupedFilteredTimeEntriesProvider : IService
{
    Task<IEnumerable<FilteredTimeEntryItemContract>> ExecuteAsync(TimeEntriesFilter filter);
}

public class GroupedFilteredTimeEntriesProvider : IGroupedFilteredTimeEntriesProvider
{
    private readonly IFilteredTimeEntriesQuery query;
    private readonly IFilteredTimeEntriesGrouper timeEntriesGrouper;
    private readonly IFilteredTimeEntriesGroupsSorter sorter;

    private IEnumerable<FilteredTimeEntryItemContract> groupedTimeEntries;

    public GroupedFilteredTimeEntriesProvider(
        IFilteredTimeEntriesQuery query,
        IFilteredTimeEntriesGrouper timeEntriesGrouper,
        IFilteredTimeEntriesGroupsSorter sorter)
    {
        this.query = query;
        this.timeEntriesGrouper = timeEntriesGrouper;
        this.sorter = sorter;
    }

    public async Task<IEnumerable<FilteredTimeEntryItemContract>> ExecuteAsync(TimeEntriesFilter filter)
    {
        TimeEntriesFilter decoratedFilter = TimeEntriesFilterDecorator.DecorateTimeEntriesFilter(filter);
        IEnumerable<FilteredTimeEntryItemContract> timeEntries = await query.ExecuteAsync(decoratedFilter);

        groupedTimeEntries = timeEntriesGrouper.GetGroupedTimeEntries(timeEntries, filter);

        groupedTimeEntries = sorter.ApplySorts(groupedTimeEntries, filter.AppliedSortings);

        return groupedTimeEntries.Skip(filter.Skip).Take(filter.Take);
    }
}
