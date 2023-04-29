using MediatR;
using MediatR.Extensions.AttributedBehaviors;
using System;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.AddTimeEntry;

[MediatRBehavior(typeof(AddTimeEntryValidationExistenceBehavior))]
public class AddTimeEntryCommand : IRequest<Unit>
{
    public string Description { get; set; }
    public string TaskName { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset StopTime { get; set; }
    public int ProjectId { get; set; }
    public int TimeTrackerAccountId { get; set; }
    public bool IsApproved { get; set; }
}
