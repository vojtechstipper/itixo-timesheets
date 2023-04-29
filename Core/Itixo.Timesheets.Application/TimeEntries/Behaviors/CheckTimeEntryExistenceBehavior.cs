using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Application.TimeEntries.States.Queries.StateChangesByTimeEntryId;
using Itixo.Timesheets.Shared.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.TimeEntries.Behaviors;

public interface ITimeEntryIdRequest
{
    public int TimeEntryId { get; set; }
}

public class CheckTimeEntryExistenceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : ITimeEntryIdRequest,IRequest<TResponse>
    where TResponse: StateChangesByTimeEntryIdResponse
{
    private readonly ITimeEntryRepository timeEntryRepository;

    public CheckTimeEntryExistenceBehavior(ITimeEntryRepository timeEntryRepository)
    {
        this.timeEntryRepository = timeEntryRepository;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!await timeEntryRepository.AnyAsync(request.TimeEntryId))
        {
            throw new NotFoundException($"TimeEntry with Id {request.TimeEntryId} Not Found.");
        }

        return await next();
    }

}
