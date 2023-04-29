using System;
using System.Collections.Generic;
using System.Linq;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Tamarack.Pipeline;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;

public class SummaryApprovedTimeEntriesHandler : IFilter<TimeTrackerAccountReportGridItemDtoBuilder, TimeTrackerAccountReportGridItemDtoBuilder>
{
    private readonly List<TimeEntriesDurationContract> approvedTimeEntries;
    private readonly TimeTrackerAccountsReportsQueryFilter filter;

    public SummaryApprovedTimeEntriesHandler(List<TimeEntriesDurationContract> approvedTimeEntries, TimeTrackerAccountsReportsQueryFilter filter)
    {
        this.approvedTimeEntries = approvedTimeEntries;
        this.filter = filter;
    }

    public TimeTrackerAccountReportGridItemDtoBuilder Execute(
        TimeTrackerAccountReportGridItemDtoBuilder context,
        Func<TimeTrackerAccountReportGridItemDtoBuilder, TimeTrackerAccountReportGridItemDtoBuilder> executeNext)
    {
        var usersApprovedTimeEntries = approvedTimeEntries.Where(w => w.TimeTrackerAccountId == filter.TimeTrackerAccountId).ToList();
        context.WithSummaryDurationApprovedTimeEntries(usersApprovedTimeEntries);
        return executeNext(context);
    }
}
