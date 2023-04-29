using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Shared.Exceptions;
using MediatR;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.AddTimeEntry;

public class AddTimeEntryValidationExistenceBehavior : IPipelineBehavior<AddTimeEntryCommand, Unit>
{
    private readonly IProjectRepository projectRepository;
    private readonly ITimeTrackerAccountRepository timeTrackerAccountRepository;

    public AddTimeEntryValidationExistenceBehavior(IProjectRepository projectRepository, ITimeTrackerAccountRepository timeTrackerAccountRepository)
    {
        this.projectRepository = projectRepository;
        this.timeTrackerAccountRepository = timeTrackerAccountRepository;
    }

    public async Task<Unit> Handle(AddTimeEntryCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
    {
        if (!await projectRepository.AnyAsync(request.ProjectId))
        {
            var validationFailure = new ValidationFailure(nameof(request.ProjectId), $"Project {request.ProjectId} not exists.");
            throw new AppValidationException(new List<ValidationFailure> { validationFailure });
        }

        if (!await timeTrackerAccountRepository.AnyAsync(request.TimeTrackerAccountId))
        {
            var validationFailure = new ValidationFailure(nameof(request.TimeTrackerAccountId), $"TimeTrackerAccountId {request.TimeTrackerAccountId} not exists.");
            throw new AppValidationException(new List<ValidationFailure> { validationFailure });
        }

        return await next();
    }
}
