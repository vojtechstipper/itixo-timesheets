using System.Collections.Generic;
using Itixo.Timesheets.Contracts.TimeEntries.Bans;
using MediatR;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.BanTimeEntry;

public class BanTimeEntryCommand : IRequest<Unit>
{
    public IEnumerable<BanTimeEntryDraftContract> TimeEntries { get; set; }
        = new List<BanTimeEntryDraftContract>();
}