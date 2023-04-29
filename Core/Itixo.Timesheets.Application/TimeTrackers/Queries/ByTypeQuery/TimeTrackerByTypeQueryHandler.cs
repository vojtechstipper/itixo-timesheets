using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.TimeTrackers;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeTrackers;
using MediatR;

namespace Itixo.Timesheets.Application.TimeTrackers.Queries.ByTypeQuery;

public class TimeTrackerByTypeQueryHandler : IRequestHandler<TimeTrackerByTypeQuery, TimeTrackerByTypeQueryResponse>
{
    private readonly ITimeTrackerRepository timeTrackerRepository;
    private readonly IMapper mapper;

    public TimeTrackerByTypeQueryHandler(ITimeTrackerRepository timeTrackerRepository, IMapper mapper)
    {
        this.timeTrackerRepository = timeTrackerRepository;
        this.mapper = mapper;
    }

    public async Task<TimeTrackerByTypeQueryResponse> Handle(TimeTrackerByTypeQuery request, CancellationToken cancellationToken)
    {
        TimeTracker timeTracker = await timeTrackerRepository.GetTimeTrackerByTypeAsync(request.TimeTrackerType);
        return new TimeTrackerByTypeQueryResponse { TimeTracker = mapper.Map<TimeTrackerContract>(timeTracker) };
    }
}
