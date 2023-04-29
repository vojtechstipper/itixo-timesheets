using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Shared.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Itixo.Timesheets.Domain.TimeTrackers;
using Microsoft.EntityFrameworkCore;
using Tamarack.Pipeline;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;

public interface IReportsQueryOrchestrator : IService
{
    Task<IEnumerable<AccountReportGridItemContract>> ExecuteAsync(ReportsQueryFilter queryFilter);
}

public class ReportsQueryOrchestrator : IReportsQueryOrchestrator
{
    private readonly ITimeTrackerAccountRepository timeTrackerAccountRepository;
    private readonly IReportTimeEntryQuery reportTimeEntriesQuery;
    private readonly IMapper mapper;

    public ReportsQueryOrchestrator(ITimeTrackerAccountRepository timeTrackerAccountRepository, IReportTimeEntryQuery reportTimeEntriesQuery, IMapper mapper)
    {
        this.timeTrackerAccountRepository = timeTrackerAccountRepository;
        this.reportTimeEntriesQuery = reportTimeEntriesQuery;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<AccountReportGridItemContract>> ExecuteAsync(ReportsQueryFilter queryFilter)
    {
        List<TimeEntriesDurationContract> timeEntryBans = await mapper
            .ProjectTo<TimeEntriesDurationContract>(reportTimeEntriesQuery.GetQueryable(queryFilter).AsNoTracking().Where(w => w.StateContext.IsBan))
            .ToListAsync();

        List<TimeEntriesDurationContract> approvedTimeEntries = await mapper
            .ProjectTo<TimeEntriesDurationContract>(reportTimeEntriesQuery.GetQueryable(queryFilter).AsNoTracking().Where(w => w.StateContext.IsApproved))
            .ToListAsync();

        List<TimeEntriesDurationContract> timeEntryDrafts = await mapper
            .ProjectTo<TimeEntriesDurationContract>(reportTimeEntriesQuery.GetQueryable(queryFilter).AsNoTracking().Where(w => w.StateContext.IsDraft))
            .ToListAsync();

        var allTimeEntries = approvedTimeEntries.Concat(timeEntryDrafts.Where(ted => !approvedTimeEntries.Select(s => s.ExternalId).Contains(ted.ExternalId))).ToList();

        var results = new List<AccountReportGridItemContract>();

        List<TimeTrackerAccount> accounts = await timeTrackerAccountRepository.RawListAsync();

        foreach (TimeTrackerAccount account in accounts)
        {
            var userQueryFilter = new TimeTrackerAccountsReportsQueryFilter(queryFilter) {TimeTrackerAccountId = account.Id};

            AccountReportGridItemContract result = new Pipeline<TimeTrackerAccountReportGridItemDtoBuilder, TimeTrackerAccountReportGridItemDtoBuilder>()
                .Add(new SummaryTimeEntriesHandler(allTimeEntries, userQueryFilter))
                .Add(new SummaryApprovedTimeEntriesHandler(approvedTimeEntries, userQueryFilter))
                .Add(new SummaryBannedTimeEntriesHandler(timeEntryBans, userQueryFilter))
                .Add(new SummaryDraftedTimeEntriesHandler(timeEntryDrafts, userQueryFilter))
                .Finally(builder => builder)
                .Execute(new TimeTrackerAccountReportGridItemDtoBuilder())
                .WithUserName(account.Username)
                .WithTimeTrackerAccountId(account.Id)
                .Build();

            results.Add(result);
        }

        return results;
    }
}
