using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Contracts.Synchronization;
using Itixo.Timesheets.Domain.Synchronization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Itixo.Timesheets.Application.Synchronization.Queries.GetInvalidTimeEntriesReport;

public class GetInvalidTimeEntriesReportsHandler : IRequestHandler<GetInvalidTimeEntriesReportsQuery, GetInvalidTimeEntriesReportsResponse>
{
    private readonly IPersistenceQuery<InvalidTimeEntriesReport, int> persistenceQuery;
    private readonly IMapper mapper;

    public GetInvalidTimeEntriesReportsHandler(IPersistenceQuery<InvalidTimeEntriesReport, int> persistenceQuery, IMapper mapper)
    {
        this.persistenceQuery = persistenceQuery;
        this.mapper = mapper;
    }

    public async Task<GetInvalidTimeEntriesReportsResponse> Handle(GetInvalidTimeEntriesReportsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<InvalidTimeEntriesReport> invalidTimeEntriesReports =
            persistenceQuery.GetQueryable().Where(w => w.ReceiverEmailAddress == request.ReceiverEmailAddress);

        InvalidTimeEntriesReportContract contract =
            await mapper.ProjectTo<InvalidTimeEntriesReportContract>(invalidTimeEntriesReports).FirstOrDefaultAsync(cancellationToken);

        return new GetInvalidTimeEntriesReportsResponse {InvalidTimeEntriesReport = contract};
    }
}
