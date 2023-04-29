using AutoMapper;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Contracts.TimeEntries.States;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Domain.TimeEntries.States;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.TimeEntries.States.Queries.StateChangesByTimeEntryId;

public class StateChangesByTimeEntryIdQueryHandler : IRequestHandler<StateChangesByTimeEntryIdQuery, StateChangesByTimeEntryIdResponse>
{
    private readonly IPersistenceQuery<TimeEntry, int> timeEntryQuery;
    private readonly IMapper mapper;

    public StateChangesByTimeEntryIdQueryHandler(IMapper mapper, IPersistenceQuery<TimeEntry, int> timeEntryQuery)
    {
        this.mapper = mapper;
        this.timeEntryQuery = timeEntryQuery;
    }

    public async Task<StateChangesByTimeEntryIdResponse> Handle(StateChangesByTimeEntryIdQuery request, CancellationToken cancellationToken)
    {
        IQueryable<TimeEntryStateChange> entities = timeEntryQuery.GetQueryable().Where(w => w.Id == request.TimeEntryId)
            .SelectMany(s => s.StateContext.TimeEntryStateChanges);

        List<TimeEntryStateChangeContract> contracts = await mapper.ProjectTo<TimeEntryStateChangeContract>(entities).ToListAsync(cancellationToken);
        return new StateChangesByTimeEntryIdResponse { TimeEntryStateChanges = contracts };
    }
}
