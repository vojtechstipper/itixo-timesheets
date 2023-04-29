using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Services;
using MediatR;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.UpdateCommand;

public class UpdateTimeTrackerAccountCommandHandler : IRequestHandler<UpdateTimeTrackerAccountContract, AddOrUpdateTimeTrackerAccountResult>
{
    private readonly ITimeTrackerAccountRepository timeTrackerAccountRepository;
    private readonly ICurrentIdentityProvider currentIdentityProvider;

    public UpdateTimeTrackerAccountCommandHandler(ITimeTrackerAccountRepository timeTrackerAccountRepository, ICurrentIdentityProvider currentIdentityProvider)
    {
        this.timeTrackerAccountRepository = timeTrackerAccountRepository;
        this.currentIdentityProvider = currentIdentityProvider;
    }

    public async Task<AddOrUpdateTimeTrackerAccountResult> Handle(UpdateTimeTrackerAccountContract request, CancellationToken cancellationToken)
    {
        request.CurrentIdentityExternalId = currentIdentityProvider.ExternalId;
        TimeTrackerAccount timeTrackerAccount = await timeTrackerAccountRepository.UnauthorizedGetAsync(request.Id);
        timeTrackerAccount.Update(request);
        await timeTrackerAccountRepository.UpdateAsync(timeTrackerAccount);
        return new AddOrUpdateTimeTrackerAccountResult{Id = timeTrackerAccount.Id};
    }
}
