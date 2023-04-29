using System;
using System.Collections.Generic;
using System.Linq;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Tamarack.Pipeline;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;

public class SummaryTimeEntriesHandler : IFilter<TimeTrackerAccountReportGridItemDtoBuilder, TimeTrackerAccountReportGridItemDtoBuilder>
{
    private readonly List<TimeEntriesDurationContract> allTimeEntriesDuration;
    private readonly TimeTrackerAccountsReportsQueryFilter filter;

    public SummaryTimeEntriesHandler(List<TimeEntriesDurationContract> allTimeEntriesDuration, TimeTrackerAccountsReportsQueryFilter filter)
    {
        this.allTimeEntriesDuration = allTimeEntriesDuration;
        this.filter = filter;
    }

    public TimeTrackerAccountReportGridItemDtoBuilder Execute(
        TimeTrackerAccountReportGridItemDtoBuilder context,
        Func<TimeTrackerAccountReportGridItemDtoBuilder, TimeTrackerAccountReportGridItemDtoBuilder> executeNext)
    {
        var usersAllTimeEntries = allTimeEntriesDuration.Where(w => w.TimeTrackerAccountId == filter.TimeTrackerAccountId).ToList();
        context.WithSummaryDurationAllEntries(usersAllTimeEntries);
        return executeNext(context);
    }
}
