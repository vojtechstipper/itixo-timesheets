using System;
using System.Collections.Generic;
using System.Linq;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Shared.Extensions;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;

public class TimeTrackerAccountReportGridItemDtoBuilder
{
    private readonly AccountReportGridItemContract result;

    public TimeTrackerAccountReportGridItemDtoBuilder()
    {
        result = new AccountReportGridItemContract();
    }

    public AccountReportGridItemContract Build()
    {
        return result;
    }

    public TimeTrackerAccountReportGridItemDtoBuilder WithSummaryDurationAllEntries(IEnumerable<TimeEntriesDurationContract> timeEntryDurations)
    {
        result.SummaryDurationAllEntries = SummarizeDurations(timeEntryDurations);
        return this;
    }

    public TimeTrackerAccountReportGridItemDtoBuilder WithSummaryDurationApprovedTimeEntries(IEnumerable<TimeEntriesDurationContract> timeEntryDurations)
    {
        result.SummaryDurationApproves = SummarizeDurations(timeEntryDurations);
        return this;
    }

    public TimeTrackerAccountReportGridItemDtoBuilder WithSummaryDurationBannedEntries(IEnumerable<TimeEntriesDurationContract> timeEntryDurations)
    {
        result.SummaryDurationBans = SummarizeDurations(timeEntryDurations);
        return this;
    }

    public TimeTrackerAccountReportGridItemDtoBuilder WithSummaryDurationDraftedEntries(IEnumerable<TimeEntriesDurationContract> timeEntryDurations)
    {
        result.SummaryDurationDrafts = SummarizeDurations(timeEntryDurations);
        return this;
    }

    private static TimeSpan SummarizeDurations(IEnumerable<TimeEntriesDurationContract> timeEntryDurations)
        =>  TimeSpanExtensions.Sum(timeEntryDurations.Select(s => s.Duration).ToArray());

    public TimeTrackerAccountReportGridItemDtoBuilder WithUserName(string username)
    {
        result.Username = username;
        return this;
    }

    public TimeTrackerAccountReportGridItemDtoBuilder WithTimeTrackerAccountId(int accountId)
    {
        result.TimeTrackerAccountId = accountId;
        return this;
    }
}
