using FluentValidation;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Application.Configurations.Commands.AddSyncDateRange;

public class AddSyncDateRangeValidator : AbstractValidator<AddSyncDateRangeCommand>
{
    public AddSyncDateRangeValidator()
    {
        RuleFor(x => x.StopSyncBusinessDaysAgoValue)
            .GreaterThan(0)
            .WithMessage(nameof(Validations.Common_IntegerValidation_Value_More_Than_Zero));

        RuleFor(x => x.StartSyncBusinessDaysAgoValue)
            .GreaterThan(0)
            .WithMessage(nameof(Validations.Common_IntegerValidation_Value_More_Than_Zero))
            .GreaterThanOrEqualTo(x => x.StopSyncBusinessDaysAgoValue)
            .WithMessage(nameof(Validations.SyncBusinessDays_ValidationMessage_Start_Must_Be_Greater_Tan_Stop));
    }
}
