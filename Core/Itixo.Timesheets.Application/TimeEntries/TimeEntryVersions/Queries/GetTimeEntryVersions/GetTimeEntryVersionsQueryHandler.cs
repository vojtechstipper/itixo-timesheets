using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Contracts.TimeEntries.Versions;
using Itixo.Timesheets.Domain.TimeEntries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Itixo.Timesheets.Application.TimeEntries.TimeEntryVersions.Queries.GetTimeEntryVersions;

public class GetTimeEntryVersionsQueryHandler : IRequestHandler<GetTimeEntryVersionsQuery, GetTimeEntryVersionsQueryResult>
{
    private readonly IMapper mapper;
    private readonly IPersistenceQuery<TimeEntryVersion, int> timeEntryVersionsQuery;

    public GetTimeEntryVersionsQueryHandler(
        IMapper mapper,
        IPersistenceQuery<TimeEntryVersion, int> timeEntryVersionsQuery)
    {
        this.mapper = mapper;
        this.timeEntryVersionsQuery = timeEntryVersionsQuery;
    }

    public async Task<GetTimeEntryVersionsQueryResult> Handle(GetTimeEntryVersionsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<TimeEntryVersion> entryVersions =
            timeEntryVersionsQuery
            .GetQueryable()
            .Include(x => x.TimeEntryParams.Project)
            .Include(x => x.PreviousTimeEntryVersion)
            .Include(x => x.PreviousTimeEntryVersion.TimeEntryParams.Project)
            .AsNoTracking()
            .Where(w => w.TimeEntry.Id == request.TimeEntryId)
            .OrderByDescending(o => o.ImportedDate);

        List<TimeEntryVersionContract> timeEntryVersions = await
            mapper.ProjectTo<TimeEntryVersionContract>(entryVersions).ToListAsync(cancellationToken);

        return new GetTimeEntryVersionsQueryResult{ TimeEntryVersions = timeEntryVersions };
    }
}
