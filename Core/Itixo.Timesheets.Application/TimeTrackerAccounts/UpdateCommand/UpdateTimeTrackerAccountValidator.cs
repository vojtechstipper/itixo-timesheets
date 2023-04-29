using FluentValidation;
using FluentValidation.Validators;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Domain.TimeTrackers;
using System;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.UpdateCommand;

public class UpdateTimeTrackerAccountValidator : AbstractValidator<UpdateTimeTrackerAccountContract>
{
    private readonly IPersistenceQuery<TimeTrackerAccount, int> timeTrackerAccountQuery;
    public UpdateTimeTrackerAccountValidator(IPersistenceQuery<TimeTrackerAccount, int> timeTrackerAccountQuery)
    {
        this.timeTrackerAccountQuery = timeTrackerAccountQuery;
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.ExternalId).NotEmpty();
        RuleFor(x => x).CustomAsync(ValidateExistenceAsync);
    }

    private async Task ValidateExistenceAsync(UpdateTimeTrackerAccountContract contract, ValidationContext<UpdateTimeTrackerAccountContract> context, CancellationToken cancellationToken)
    {
        if (!await timeTrackerAccountQuery.GetQueryable().AnyAsync(f => f.Id == contract.Id, cancellationToken: cancellationToken))
        {
            throw new NotFoundException($"TimeTrackerAccount with Id {contract.Id} not found");
        }
    }
}
