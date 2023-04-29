using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Enums;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.AddCommand;

public class AddTimeTrackerAccountValidator : AbstractValidator<AddTimeTrackerAccountContract>
{
    private readonly ITimeTrackerRepository timeTrackerRepository;

    public AddTimeTrackerAccountValidator(ITimeTrackerRepository timeTrackerRepository)
    {
        this.timeTrackerRepository = timeTrackerRepository;

        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.TimeTrackerId).NotEmpty();
        RuleFor(x => x).CustomAsync(ValidateExternalIdAsync);
    }

    private async Task ValidateExternalIdAsync(AddTimeTrackerAccountContract contract, ValidationContext<AddTimeTrackerAccountContract> context, CancellationToken token)
    {
        TimeTracker timeTracker = await timeTrackerRepository.GetByIdAsync(contract.TimeTrackerId, token);

        if (timeTracker.Type != TimeTrackerType.ThisApplication && string.IsNullOrWhiteSpace(contract.ExternalId))
        {
            context.AddFailure(nameof(AddTimeTrackerAccountContract.ExternalId), nameof(Validations.UserDetailModel_ValidationMessage_ExternalId_is_Required));
        }
    }
}
