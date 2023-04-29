using FluentValidation;
using Itixo.Timesheets.Contracts.TimeEntries;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Application.TimeEntries.FilteredQuery;

public class TimeEntriesFilterValidator : AbstractValidator<TimeEntriesFilter>
{
    public TimeEntriesFilterValidator()
    {
        RuleFor(f => f.FromDate).LessThanOrEqualTo(f => f.ToDate)
            .When(x => x.FromDate != default && x.ToDate != default)
            .WithMessage(nameof(Validations.TriggerSynchronization_ValidationMesssage_StartDate_Must_Be_Greater_Than_EndDate));
    }
}
