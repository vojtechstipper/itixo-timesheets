using MediatR;

namespace Itixo.Timesheets.Application.Synchronization.Queries.GetInvalidTimeEntriesReport;

public class GetInvalidTimeEntriesReportsQuery : IRequest<GetInvalidTimeEntriesReportsResponse>
{
    public string ReceiverEmailAddress { get; set; }
}
