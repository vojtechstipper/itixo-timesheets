using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.TimeEntries.Deleted;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.DeleteTimeEntry;

public class DeleteTimeEntryHandler : IRequestHandler<DeleteTimeEntryCommand, Unit>
{
    private readonly ITimeEntryRepository timeEntryRepository;

    public DeleteTimeEntryHandler(ITimeEntryRepository timeEntryRepository)
    {
        this.timeEntryRepository = timeEntryRepository;
    }

    public async Task<Unit> Handle(DeleteTimeEntryCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<int> deleteTimeEntries = request.TimeEntries.Select(entry => entry.Id);

        if (deleteTimeEntries.Any())
        {
            await timeEntryRepository.DeleteTimeEntriesAsync(deleteTimeEntries);
        }

        return Unit.Value;
    }
}
