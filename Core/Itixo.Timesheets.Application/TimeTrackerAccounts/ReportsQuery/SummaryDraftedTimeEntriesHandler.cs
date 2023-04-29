using System;
using System.Collections.Generic;
using System.Linq;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Tamarack.Pipeline;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;

public class SummaryDraftedTimeEntriesHandler : IFilter<TimeTrackerAccountReportGridItemDtoBuilder, TimeTrackerAccountReportGridItemDtoBuilder>
{
    private readonly List<TimeEntriesDurationContract> timeEntryDrafts;
    private readonly TimeTrackerAccountsReportsQueryFilter filter;

    public SummaryDraftedTimeEntriesHandler(List<TimeEntriesDurationContract> timeEntryDrafts, TimeTrackerAccountsReportsQueryFilter filter)
    {
        this.timeEntryDrafts = timeEntryDrafts;
        this.filter = filter;
    }

    public TimeTrackerAccountReportGridItemDtoBuilder Execute(
        TimeTrackerAccountReportGridItemDtoBuilder context,
        Func<TimeTrackerAccountReportGridItemDtoBuilder, TimeTrackerAccountReportGridItemDtoBuilder> executeNext)
    {
        IEnumerable<TimeEntriesDurationContract> accountTimeEntryDrafts = timeEntryDrafts.Where(w => w.TimeTrackerAccountId == filter.TimeTrackerAccountId);
        context.WithSummaryDurationDraftedEntries(accountTimeEntryDrafts);
        return executeNext(context);
    }
}
