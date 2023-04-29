using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Shared.Exceptions;
using Itixo.Timesheets.Shared.Resources;
using MediatR;

namespace Itixo.Timesheets.Application.Configurations.Commands.UpdateSyncDateRange;

public class SyncDateRangeExistenceValidator : IPipelineBehavior<UpdateSyncDateRangeCommand, Unit>
{
    private readonly IConfigurationRepository configurationRepository;

    public SyncDateRangeExistenceValidator(IConfigurationRepository configurationRepository)
    {
        this.configurationRepository = configurationRepository;
    }

    public async Task<Unit> Handle(UpdateSyncDateRangeCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
    {
        var validationFailures = new List<ValidationFailure>();

        if (!await configurationRepository.AnyAsync(request.StartSyncBusinessDaysAgoId, cancellationToken))
        {
            var validationFailure = new ValidationFailure(
                nameof(UpdateSyncDateRangeCommand.StartSyncBusinessDaysAgoId),
                nameof(Validations.ConfigurationValidator_Configuration_With_Id_Does_Not_Exist));
            validationFailures.Add(validationFailure);
        }

        if (!await configurationRepository.AnyAsync(request.StartSyncBusinessDaysAgoId, cancellationToken))
        {
            var validationFailure = new ValidationFailure(
                nameof(UpdateSyncDateRangeCommand.StopSyncBusinessDaysAgoId),
                nameof(Validations.ConfigurationValidator_Configuration_With_Id_Does_Not_Exist));
            validationFailures.Add(validationFailure);
        }

        if (validationFailures.Count > 0)
        {
            throw new AppValidationException(validationFailures);
        }

        return await next();
    }
}
