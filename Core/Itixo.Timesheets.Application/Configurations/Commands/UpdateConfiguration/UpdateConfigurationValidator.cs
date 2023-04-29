using FluentValidation;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Application.Configurations.Commands.UpdateConfiguration;

public class UpdateConfigurationValidator : AbstractValidator<UpdateConfigurationCommand>
{
    public UpdateConfigurationValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage(nameof(Validations.ConfigurationValidator_Configuration_Value_Cannot_Be_Empty));

        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage(Validations.ConfigurationValidator_Configuration__Key_Cannot_Be_Empty);
    }
}
