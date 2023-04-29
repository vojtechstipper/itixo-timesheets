using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Contracts.SyncHistory;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Itixo.Timesheets.Application.SyncHistory.Queries.ListQuery;

public class SyncLogRecordsQueryHandler : IRequestHandler<SyncLogRecordsQuery, List<SyncLogRecordContract>>
{
    private readonly IMapper mapper;

    private readonly IPersistenceQuery<SyncLogRecord, Guid> syncLogRecordsQuery;

    public SyncLogRecordsQueryHandler(IMapper mapper, IPersistenceQuery<SyncLogRecord, Guid> syncLogRecordsQuery)
    {
        this.mapper = mapper;
        this.syncLogRecordsQuery = syncLogRecordsQuery;
    }

    public async Task<List<SyncLogRecordContract>> Handle(SyncLogRecordsQuery request, CancellationToken cancellationToken)
    {
        DateTimeOffset fromDate = request.Filter.FromDate.GetDateWithMinimumTime();
        DateTimeOffset toDate = request.Filter.ToDate.GetDateWithMaximumTime();
        List<SyncLogRecord> items = await syncLogRecordsQuery.GetQueryable()
            .IgnoreQueryFilters()
            .Include(x => x.Batches)
            .Include(x => x.IdentityInfo)
            .Where(w => w.StartedDate > fromDate && w.StoppedDate <= toDate)
            .OrderByDescending(x => x.StartedDate)
            .ToListAsync(cancellationToken);

        return items.Select(item => new SyncLogRecordContract
        {
            StartedDate = item.StartedDate,
            StoppedDate = item.StoppedDate.Value,
            Duration = TimeSpanExtensions.ToCustomHoursFormat(item.Duration),
            InsertedDraftedRecordsIds = item.Batches.SelectMany(batch => batch.InsertedDraftedRecordsIds).ToList(),
            UpdatedDraftedRecordsIds = item.Batches.SelectMany(batch => batch.UpdatedDraftedRecordsIds).ToList(),
            InsertedApprovedRecordsIds = item.Batches.SelectMany(batch => batch.InsertedApprovedRecordsIds).ToList(),
            UpdatedApprovedRecordsIds = item.Batches.SelectMany(batch => batch.UpdatedApprovedRecordsIds).ToList(),
            IdentityName = item.IdentityInfo.Identifier,
            Successful = string.IsNullOrWhiteSpace(item.ErrorMessage),
            SyncedFrom = item.SyncedRecordsFrom,
            SyncedTo = item.SyncedRecordsTo
        }).ToList();
    }
}
