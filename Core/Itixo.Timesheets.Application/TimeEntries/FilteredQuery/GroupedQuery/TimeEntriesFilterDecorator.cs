using Itixo.Timesheets.Contracts.TimeEntries;

namespace Itixo.Timesheets.Application.TimeEntries.FilteredQuery.GroupedQuery;

public static class TimeEntriesFilterDecorator
{
    public static TimeEntriesFilter DecorateTimeEntriesFilter(TimeEntriesFilter filter)
    {
        var clone = TimeEntriesFilter.Clone(filter);
        clone.Take = int.MaxValue;
        clone.Skip = 0;
        return clone;
    }
}
