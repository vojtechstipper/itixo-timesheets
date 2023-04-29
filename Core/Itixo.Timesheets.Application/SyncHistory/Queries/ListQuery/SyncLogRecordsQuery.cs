using Itixo.Timesheets.Contracts.SyncHistory;
using MediatR;
using System.Collections.Generic;

namespace Itixo.Timesheets.Application.SyncHistory.Queries.ListQuery;

public class SyncLogRecordsQuery : IRequest<List<SyncLogRecordContract>>
{
    public SyncLogRecordsFilter Filter { get; }

    public SyncLogRecordsQuery(SyncLogRecordsFilter filter)
    {
        Filter = filter;
    }
}
