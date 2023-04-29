using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Domain.TimeEntries.States;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Services;
using MediatR;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.AddTimeEntry;

public class AddTimeEntryCommandHandler : IRequestHandler<AddTimeEntryCommand, Unit>
{
    private readonly ITimeEntryRepository timeEntryRepository;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly ICurrentIdentityProvider currentIdentityProvider;
    private readonly IIdentityInfoRepository identityInfoRepository;
    private readonly IProjectRepository projectRepository;
    private readonly ITimeTrackerAccountRepository timeTrackerAccountRepository;

    public AddTimeEntryCommandHandler(
        ITimeEntryRepository timeEntryRepository,
        IDateTimeProvider dateTimeProvider,
        ICurrentIdentityProvider currentIdentityProvider,
        IIdentityInfoRepository identityInfoRepository,
        IProjectRepository projectRepository,
        ITimeTrackerAccountRepository timeTrackerAccountRepository)
    {
        this.timeEntryRepository = timeEntryRepository;
        this.dateTimeProvider = dateTimeProvider;
        this.currentIdentityProvider = currentIdentityProvider;
        this.identityInfoRepository = identityInfoRepository;
        this.projectRepository = projectRepository;
        this.timeTrackerAccountRepository = timeTrackerAccountRepository;
    }

    public async Task<Unit> Handle(AddTimeEntryCommand request, CancellationToken cancellationToken)
    {
        Project project = await projectRepository.GetByIdAsync(request.ProjectId);
        var timeEntryParams = new TimeEntryParams
        {
            StopTime = request.StopTime,
            StartTime = request.StartTime,
            Description = request.Description,
            Project = project,
            TaskName = request.TaskName
        };

        IdentityInfo identityInfo = await identityInfoRepository.GetIdentityInfoAsync(currentIdentityProvider.ExternalId);
        TimeTrackerAccount account = await timeTrackerAccountRepository.GetAsync(request.TimeTrackerAccountId);

        var transitionParams = TransitionParams.Create(identityInfo, TimeEntryStateChangeReasons.CreatedManuallyInApplication);
        var timeEntry = TimeEntry.CreateDraft(timeEntryParams, account, dateTimeProvider.Now, transitionParams);

        if (request.IsApproved)
        {
            transitionParams = TransitionParams.Create(identityInfo, TimeEntryStateChangeReasons.Ok);
            timeEntry = TimeEntry.CreateApproved(timeEntryParams, account, dateTimeProvider.Now, transitionParams);
        }
        
        int id = await timeEntryRepository.AddAsync(timeEntry);

        timeEntry.TimeEntryParams.ExternalId = id;

        await timeEntryRepository.UpdateAsync(timeEntry);

        return Unit.Value;
    }
}
