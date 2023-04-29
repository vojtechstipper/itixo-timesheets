using FluentValidation;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Application.TimeEntries.States.Queries.StateChangesByTimeEntryId;

public class StateChangesByTimeEntryIdQueryValidator : AbstractValidator<StateChangesByTimeEntryIdQuery>
{
    public StateChangesByTimeEntryIdQueryValidator()
    {
        RuleFor(f => f.TimeEntryId).GreaterThan(0)
            .WithMessage(nameof(Validations.StateChangesByTimeEntryIdQueryValidator_StateChangesByTimeEntryIdQueryValidator_TimeEntryId_must_be_greater_than_0_));
    }
}
