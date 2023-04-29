using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain.Synchronization;
using MediatR;

namespace Itixo.Timesheets.Application.Synchronization.Commands.AddOrUpdateInvalidReport;

public class AddOrUpdateInvalidTimeEntriesReportHandler : IRequestHandler<AddOrUpdateInvalidTimeEntriesReportCommand, Unit>
{
    private readonly IInvalidTimeEntriesReportRepository repository;

    public AddOrUpdateInvalidTimeEntriesReportHandler(IInvalidTimeEntriesReportRepository repository)
    {
        this.repository = repository;
    }

    public async Task<Unit> Handle(AddOrUpdateInvalidTimeEntriesReportCommand request, CancellationToken cancellationToken)
    {
        InvalidTimeEntriesReport invalidTimeEntriesReport = await repository.GetByEmailAsync(request.ReceiverEmailAddress, cancellationToken);

        if (invalidTimeEntriesReport == null)
        {
            invalidTimeEntriesReport = InvalidTimeEntriesReport.Create(request.ReceiverEmailAddress, request.LastReceivedTime, request.ExternalIds);
            await repository.InsertAsync(invalidTimeEntriesReport, cancellationToken);
        }
        else
        {
            invalidTimeEntriesReport.Update(request.ReceiverEmailAddress, request.LastReceivedTime, request.ExternalIds);
            await repository.UpdateAsync(invalidTimeEntriesReport, cancellationToken);
        }

        return default;
    }
}
