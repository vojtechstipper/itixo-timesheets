using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Domain.TimeEntries.States;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;
using Itixo.Timesheets.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.ApproveTimeEntry;

public class ApproveTimeEntryHandler : IRequestHandler<ApproveTimeEntryCommand, Unit>
{
    private readonly ICurrentIdentityProvider currentIdentityProvider;
    private readonly IPersistenceQuery<TimeEntry, int> timeEntryQuery;
    private readonly IPersistenceQuery<IdentityInfo, int> identityInfoQuery;
    private readonly ITimeEntryRepository timeEntryRepository;

    private readonly List<TimeEntry> timeEntriesToUpdate = new List<TimeEntry>();
    private List<TimeEntry> drafts = new List<TimeEntry>();

    public ApproveTimeEntryHandler(ICurrentIdentityProvider currentIdentityProvider, IPersistenceQuery<IdentityInfo, int> identityInfoQuery, IPersistenceQuery<TimeEntry, int> timeEntryQuery, ITimeEntryRepository timeEntryRepository)
    {
        this.currentIdentityProvider = currentIdentityProvider;
        this.identityInfoQuery = identityInfoQuery;
        this.timeEntryQuery = timeEntryQuery;
        this.timeEntryRepository = timeEntryRepository;
    }


    public async Task<Unit> Handle(ApproveTimeEntryCommand request, CancellationToken cancellationToken)
    {
        IdentityInfo currentIdentityInfo = await identityInfoQuery
            .GetQueryable()
            .FirstAsync(f => f.ExternalId == currentIdentityProvider.ExternalId, cancellationToken: cancellationToken);

        await LoadNeccessaryDataAsync(request.TimeEntries.ToList());

        foreach (TimeEntry draft in drafts)
        {
            var transitionParams = TransitionParams.Create(currentIdentityInfo, TimeEntryStateChangeReasons.Ok);
            draft.StateContext.CurrentState.ToApprove(draft.StateContext, transitionParams);
            timeEntriesToUpdate.Add(draft);
        }

        await timeEntryRepository.UpdateRangeAsync(timeEntriesToUpdate);

        return Unit.Value;
    }

    private async Task LoadNeccessaryDataAsync(List<ApproveTimeEntryDraftContract> approvalContracts)
    {
        drafts = await timeEntryQuery.GetQueryable()
            .Include(x => x.TimeTrackerAccount)
            .Include(x => x.TimeEntryParams.Project)
            .Include(x => x.Invoice)
            .Where(f => approvalContracts.Select(s => s.TimeEntryDraftId).Contains(f.Id))
            .ToListAsync();
    }
}
