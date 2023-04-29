using Itixo.Timesheets.Contracts.TimeEntries.Deleted;
using MediatR;
using System.Collections.Generic;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.DeleteTimeEntry;

public class DeleteTimeEntryCommand : IRequest<Unit>
{
    public IEnumerable<DeleteTimeEntryContract> TimeEntries { get; set; }
           = new List<DeleteTimeEntryContract>();
}
