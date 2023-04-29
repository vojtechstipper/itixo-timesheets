using FluentValidation;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Application.TimeEntries.TimeEntryVersions.Queries.GetTimeEntryVersions;

public class GetTimeEntryVersionsQueryValidator : AbstractValidator<GetTimeEntryVersionsQuery>
{
    public GetTimeEntryVersionsQueryValidator()
    {
        RuleFor(x => x.TimeEntryId)
            .GreaterThan(0)
            .WithMessage(nameof(Validations
                .GetTimeEntryVersionsQueryValidator_GetTimeEntryVersionsQueryValidator_TimeEntryId_must_be_greater_than_zero));
    }
}
