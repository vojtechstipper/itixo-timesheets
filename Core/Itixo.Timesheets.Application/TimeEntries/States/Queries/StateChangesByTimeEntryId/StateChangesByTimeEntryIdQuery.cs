using Itixo.Timesheets.Application.TimeEntries.Behaviors;
using MediatR;
using MediatR.Extensions.AttributedBehaviors;

namespace Itixo.Timesheets.Application.TimeEntries.States.Queries.StateChangesByTimeEntryId;

[MediatRBehavior(typeof(CheckTimeEntryExistenceBehavior<StateChangesByTimeEntryIdQuery, StateChangesByTimeEntryIdResponse>))]
public class StateChangesByTimeEntryIdQuery : IRequest<StateChangesByTimeEntryIdResponse>, ITimeEntryIdRequest
{
    public int TimeEntryId { get; set; }
}
