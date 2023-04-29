using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.TimeEntries.Services;

public interface IAssignInvoiceToTimeEntriesService : IService
{
    Task<AssignInvoiceResult> AssignApproves(Invoice invoice, ITimeTrackerAccountsReportsQueryFilter queryFilter);
    Task<AssignInvoiceResult> AssignDrafts(Invoice invoice, ITimeTrackerAccountsReportsQueryFilter queryFilter);
    Task<AssignInvoiceResult> AssignBans(Invoice invoice, ITimeTrackerAccountsReportsQueryFilter queryFilter);
    Task<AssignInvoiceResult> Assign(Invoice invoice, IEnumerable<int> timeEntryIds);
}

public class AssignInvoiceToTimeEntriesService : IAssignInvoiceToTimeEntriesService
{
    private readonly IPersistenceQuery<TimeEntry, int> timeEntryPersistenceQuery;
    private readonly ITimeEntryRepository timeEntryRepository;
    private readonly IReportTimeEntryQuery reportTimeEntryQuery;

    public AssignInvoiceToTimeEntriesService(IReportTimeEntryQuery reportTimeEntryQuery, IPersistenceQuery<TimeEntry, int> timeEntryPersistenceQuery, ITimeEntryRepository timeEntryRepository)
    {
        this.reportTimeEntryQuery = reportTimeEntryQuery;
        this.timeEntryPersistenceQuery = timeEntryPersistenceQuery;
        this.timeEntryRepository = timeEntryRepository;
    }

    public async Task<AssignInvoiceResult> AssignBans(Invoice invoice, ITimeTrackerAccountsReportsQueryFilter queryFilter)
    {
        IQueryable<TimeEntry> timeEntryQuery = reportTimeEntryQuery.GetQueryable(queryFilter).Where(w => w.StateContext.IsBan);
        return await AssignCore(invoice, queryFilter, timeEntryQuery);
    }

    public async Task<AssignInvoiceResult> AssignDrafts(Invoice invoice, ITimeTrackerAccountsReportsQueryFilter queryFilter)
    {
        IQueryable<TimeEntry> timeEntryQuery = reportTimeEntryQuery.GetQueryable(queryFilter).Where(w => w.StateContext.IsDraft);
        return await AssignCore(invoice, queryFilter, timeEntryQuery);
    }

    public async Task<AssignInvoiceResult> AssignApproves(Invoice invoice, ITimeTrackerAccountsReportsQueryFilter queryFilter)
    {
        IQueryable<TimeEntry> timeEntryQuery = reportTimeEntryQuery.GetQueryable(queryFilter).Where(w => w.StateContext.IsApproved);
        return await AssignCore(invoice, queryFilter, timeEntryQuery);
    }

    private async Task<AssignInvoiceResult> AssignCore(Invoice invoice, ITimeTrackerAccountsReportsQueryFilter queryFilter, IQueryable<TimeEntry> timeEntryQuery)
    {
        List<TimeEntry> timeEntries = await timeEntryQuery
            .Where(timeEntry => timeEntry.TimeTrackerAccountId == queryFilter.TimeTrackerAccountId)
            .Where(timeEntry => timeEntry.Invoice == null)
            .ToListAsync();

        await AssignInvoiceToTimeEntries(invoice, timeEntries);

        return new AssignInvoiceResult(timeEntries.Count);
    }

    public async Task<AssignInvoiceResult> Assign(Invoice invoice, IEnumerable<int> timeEntryIds)
    {
        IQueryable<TimeEntry> timeEntryQuery = timeEntryPersistenceQuery.GetQueryable().Where(timeEntry => timeEntryIds.Contains(timeEntry.Id));
        List<TimeEntry> timeEntries = await timeEntryQuery.ToListAsync();
        await AssignInvoiceToTimeEntries(invoice, timeEntries);
        return new AssignInvoiceResult(timeEntries.Count);
    }

    private async Task AssignInvoiceToTimeEntries(Invoice invoice, List<TimeEntry> timeEntries)
    {
        foreach (TimeEntry timeEntry in timeEntries)
        {
            timeEntry.Invoice = invoice;
        }

        await timeEntryRepository.UpdateRangeAsync(timeEntries);
    }
}
