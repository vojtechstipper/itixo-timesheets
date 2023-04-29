using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.TimeEntries.Bans;
using MediatR;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.BanTimeEntry;

public class BanTimeEntryHandler : IRequestHandler<BanTimeEntryCommand, Unit>
{
    private readonly ITimeEntryRepository timeEntryRepository;

    public BanTimeEntryHandler(ITimeEntryRepository timeEntryRepository)
    {
        this.timeEntryRepository = timeEntryRepository;
    }

    public async Task<Unit> Handle(BanTimeEntryCommand request, CancellationToken cancellationToken)
    {
        foreach (BanTimeEntryDraftContract input in request.TimeEntries)
        {
            await timeEntryRepository.BanTimeEntryDraftAsync(input.Id);
        }

        return Unit.Value;
    }
}