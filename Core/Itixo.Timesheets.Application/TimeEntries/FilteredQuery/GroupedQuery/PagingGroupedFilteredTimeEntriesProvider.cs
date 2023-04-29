using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Shared.Abstractions;

namespace Itixo.Timesheets.Application.TimeEntries.FilteredQuery.GroupedQuery;

public interface IPagingGroupedFilteredTimeEntriesCounter : IService
{
    Task<int> ExecuteAsync(TimeEntriesFilter filter);
}

public class PagingGroupedFilteredTimeEntriesCounter : IPagingGroupedFilteredTimeEntriesCounter
{
    private readonly IBaseFilteredTimeEntriesQuery query;

    private TimeEntriesFilter filter;

    public PagingGroupedFilteredTimeEntriesCounter(IBaseFilteredTimeEntriesQuery query)
    {
        this.query = query;
    }

    public async Task<int> ExecuteAsync(TimeEntriesFilter filter)
    {
        this.filter = TimeEntriesFilterDecorator.DecorateTimeEntriesFilter(filter);

        IEnumerable<PagingFilteredTimeEntryItemContract> timeEntries = (await query.GetFilteredResults<PagingFilteredTimeEntryItemContract>(this.filter)).ToList();

        var groupedTimeEntries = FilteredTimeEntriesGrouper.Group(timeEntries);

        IEnumerable<string> showedGroupsExternalIds = filter.IncludingChildrenGroupIds.SelectMany(groupId => groupId.Split("#"));

        IEnumerable<PagingFilteredTimeEntryItemContract> showedGroupTimeEntries = timeEntries.Where(te => showedGroupsExternalIds.Contains(te.ExternalId.ToString()));

        return groupedTimeEntries.Select(_ => _).Count() + (showedGroupTimeEntries.Count() - filter.IncludingChildrenGroupIds.Count);
    }
}
