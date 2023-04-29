using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Services;
using MediatR;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.AddCommand;

public class AddTimeTrackerAccountCommandHandler : IRequestHandler<AddTimeTrackerAccountContract, AddOrUpdateTimeTrackerAccountResult>
{
    private readonly ITimeTrackerAccountRepository timeTrackerAccountRepository;
    private readonly ICurrentIdentityProvider currentIdentityProvider;

    public AddTimeTrackerAccountCommandHandler(ITimeTrackerAccountRepository timeTrackerAccountRepository, ICurrentIdentityProvider currentIdentityProvider)
    {
        this.timeTrackerAccountRepository = timeTrackerAccountRepository;
        this.currentIdentityProvider = currentIdentityProvider;
    }

    public async Task<AddOrUpdateTimeTrackerAccountResult> Handle(AddTimeTrackerAccountContract request, CancellationToken cancellationToken)
    {
        request.CurrentIdentityExternalId = currentIdentityProvider.ExternalId;
        var account = TimeTrackerAccount.Create(request);
        await timeTrackerAccountRepository.InsertAsync(account);
        return new AddOrUpdateTimeTrackerAccountResult { Id = account.Id };
    }
}
