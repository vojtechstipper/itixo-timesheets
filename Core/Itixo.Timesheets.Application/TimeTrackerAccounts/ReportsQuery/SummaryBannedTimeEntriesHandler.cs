using System;
using System.Collections.Generic;
using System.Linq;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Tamarack.Pipeline;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;

public class SummaryBannedTimeEntriesHandler : IFilter<TimeTrackerAccountReportGridItemDtoBuilder, TimeTrackerAccountReportGridItemDtoBuilder>
{
    private readonly List<TimeEntriesDurationContract> timeEntryBans;
    private readonly TimeTrackerAccountsReportsQueryFilter filter;

    public SummaryBannedTimeEntriesHandler(List<TimeEntriesDurationContract> timeEntryBans, TimeTrackerAccountsReportsQueryFilter filter)
    {
        this.timeEntryBans = timeEntryBans;
        this.filter = filter;
    }

    public TimeTrackerAccountReportGridItemDtoBuilder Execute(
        TimeTrackerAccountReportGridItemDtoBuilder context,
        Func<TimeTrackerAccountReportGridItemDtoBuilder, TimeTrackerAccountReportGridItemDtoBuilder> executeNext)
    {
        IEnumerable<TimeEntriesDurationContract> accountsBannedTimeEntries = timeEntryBans.Where(w => w.TimeTrackerAccountId == filter.TimeTrackerAccountId);
        context.WithSummaryDurationBannedEntries(accountsBannedTimeEntries);
        return executeNext(context);
    }
}
