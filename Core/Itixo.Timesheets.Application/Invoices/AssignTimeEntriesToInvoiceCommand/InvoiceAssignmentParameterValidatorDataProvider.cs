using AutoMapper;
using Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;
using Itixo.Timesheets.Contracts.Invoices;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.Invoices.AssignTimeEntriesToInvoiceCommand;

public interface IInvoiceAssignmentParameterValidatorDataProvider : IService
{
    Task<List<TimeEntriesDurationContract>> GetBannedTimeEntriesDurationContracts(SummaryTimeEntriesInvoiceAssignmentParameter value, CancellationToken token = default);
    Task<List<TimeEntriesDurationContract>> GetDraftedTimeEntriesDurationContracts(SummaryTimeEntriesInvoiceAssignmentParameter value, CancellationToken token = default);
    Task<List<TimeEntriesDurationContract>> GetApprovedTimeEntryDurationContracts(SummaryTimeEntriesInvoiceAssignmentParameter value, CancellationToken token = default);
}

public class InvoiceAssignmentParameterValidatorDataProvider : IInvoiceAssignmentParameterValidatorDataProvider
{
    private readonly IReportTimeEntryQuery reportTimeEntriesQuery;
    private readonly IMapper mapper;

    public InvoiceAssignmentParameterValidatorDataProvider(IReportTimeEntryQuery reportTimeEntriesQuery, IMapper mapper)
    {
        this.reportTimeEntriesQuery = reportTimeEntriesQuery;
        this.mapper = mapper;
    }

    public async Task<List<TimeEntriesDurationContract>> GetBannedTimeEntriesDurationContracts(SummaryTimeEntriesInvoiceAssignmentParameter value, CancellationToken token)
        => await mapper.ProjectTo<TimeEntriesDurationContract>(reportTimeEntriesQuery.GetQueryable(value).AsNoTracking().Where(w => w.StateContext.IsBan)).ToListAsync(token);

    public async Task<List<TimeEntriesDurationContract>> GetDraftedTimeEntriesDurationContracts(SummaryTimeEntriesInvoiceAssignmentParameter value, CancellationToken token) =>
        await mapper
            .ProjectTo<TimeEntriesDurationContract>(reportTimeEntriesQuery.GetQueryable(value).AsNoTracking().Where(w => w.StateContext.IsDraft))
            .ToListAsync(token);

    public async Task<List<TimeEntriesDurationContract>> GetApprovedTimeEntryDurationContracts(SummaryTimeEntriesInvoiceAssignmentParameter value, CancellationToken token)
        => await mapper.ProjectTo<TimeEntriesDurationContract>(reportTimeEntriesQuery.GetQueryable(value).AsNoTracking().Where(w => w.StateContext.IsApproved)).ToListAsync(token);
}
