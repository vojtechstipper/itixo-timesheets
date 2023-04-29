using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using Itixo.Timesheets.Application.Configurations.Commands.AddConfiguration;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Shared.Exceptions;
using Itixo.Timesheets.Shared.Resources;
using MediatR;

namespace Itixo.Timesheets.Application.Configurations.Commands.Behaviors;

public class ConfigurationKeyUniquenessValidator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IConfigurationKeyUniquenessValidable,IRequest<TResponse>
{
    private readonly IConfigurationRepository configurationRepository;

    public ConfigurationKeyUniquenessValidator(IConfigurationRepository configurationRepository)
    {
        this.configurationRepository = configurationRepository;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!await configurationRepository.IsUniqueAsync(request.Key))
        {
            var validationFailure = new ValidationFailure(nameof(request.Key),
                nameof(Validations.ConfigurationKeyUniquenessValidator_Handle_Configuration_key_Unique));
            throw new AppValidationException(new List<ValidationFailure> { validationFailure });
        }

        return await next();
    }

}

public interface IConfigurationKeyUniquenessValidable
{
    public string Key { get; set; }
}
