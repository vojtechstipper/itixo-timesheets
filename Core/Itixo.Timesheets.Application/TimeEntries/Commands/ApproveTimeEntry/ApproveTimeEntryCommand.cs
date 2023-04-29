using System.Collections.Generic;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using MediatR;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.ApproveTimeEntry;

public class ApproveTimeEntryCommand : IRequest<Unit>
{
    public IEnumerable<ApproveTimeEntryDraftContract> TimeEntries { get; set; }
        = new List<ApproveTimeEntryDraftContract>();
}