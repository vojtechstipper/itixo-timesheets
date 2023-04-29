using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Domain;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Domain.TimeTrackers;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.AddOrAssignCommand;

public class AddOrUpdateTimeTrackerAccountHandler : IRequestHandler<AddOrUpdateTimeTrackerAccountContract, AddOrUpdateTimeTrackerAccountResult>
{
    private readonly ITimeTrackerAccountRepository timeTrackerAccountRepository;
    private readonly IMediator mediator;

    public AddOrUpdateTimeTrackerAccountHandler(ITimeTrackerAccountRepository timeTrackerAccountRepository, IMediator mediator)
    {
        this.timeTrackerAccountRepository = timeTrackerAccountRepository;
        this.mediator = mediator;
    }

    public async Task<AddOrUpdateTimeTrackerAccountResult> Handle(AddOrUpdateTimeTrackerAccountContract request, CancellationToken cancellationToken)
    {
        TimeTrackerAccount timeTrackerAccount = await timeTrackerAccountRepository
            .UnauthorizedGetByParamsAsync(request.Username, request.ExternalId);

        if (timeTrackerAccount == null)
        {
            AddTimeTrackerAccountContract addContract = request.CreateAddContract();
            return await mediator.Send(addContract, cancellationToken);
        }

        UpdateTimeTrackerAccountContract updateContract = request.CreateUpdateContract(timeTrackerAccount.Id);
        return await mediator.Send(updateContract, cancellationToken);
    }
}
