using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Application.Services.Shared;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeTrackers;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.Behaviors;

public class TogglSyncIdentityInfoToAccountAssigner<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TResponse : AddOrUpdateTimeTrackerAccountResult
where TRequest : IRequest<TResponse>
{
    private readonly ITogglSyncIdentityProvider togglSyncIdentityProvider;
    private readonly ITimeTrackerAccountRepository timeTrackerAccountRepository;

    public TogglSyncIdentityInfoToAccountAssigner(ITogglSyncIdentityProvider togglSyncIdentityProvider, ITimeTrackerAccountRepository timeTrackerAccountRepository)
    {
        this.togglSyncIdentityProvider = togglSyncIdentityProvider;
        this.timeTrackerAccountRepository = timeTrackerAccountRepository;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse result = await next();

        IdentityInfo togglSyncIdentityInfo = await togglSyncIdentityProvider.GetOrCreateIdentityAsync();
        TimeTrackerAccount timeTrackerAccount = await timeTrackerAccountRepository.GetAsync(result.Id);

        if (timeTrackerAccount.Identities.All(x => x.IdentityInfoId != togglSyncIdentityInfo.Id))
        {
            timeTrackerAccount.AssignIdentityToAccount(togglSyncIdentityInfo);
            await timeTrackerAccountRepository.UpdateAsync(timeTrackerAccount);
        }

        return result;
    }
}
