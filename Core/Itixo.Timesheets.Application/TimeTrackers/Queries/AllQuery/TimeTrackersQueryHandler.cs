using AutoMapper;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Contracts.TimeTrackers;
using Itixo.Timesheets.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Domain.TimeTrackers;

namespace Itixo.Timesheets.Application.TimeTrackers.Queries.AllQuery;

public class TimeTrackersQueryHandler : IRequestHandler<TimeTrackersQuery, TimeTrackerQueryResponse>
{
    private readonly IPersistenceQuery<TimeTracker, int> timeTrackerQuery;
    private readonly IMapper mapper;

    public TimeTrackersQueryHandler(IMapper mapper, IPersistenceQuery<TimeTracker, int> timeTrackerQuery)
    {
        this.mapper = mapper;
        this.timeTrackerQuery = timeTrackerQuery;
    }

    public async Task<TimeTrackerQueryResponse> Handle(TimeTrackersQuery request, CancellationToken token)
    {
        List<TimeTrackerContract> timeTrackers = await mapper.ProjectTo<TimeTrackerContract>(timeTrackerQuery.GetQueryable()).ToListAsync(token);
        return new TimeTrackerQueryResponse {TimeTrackers = timeTrackers};
    }
}
