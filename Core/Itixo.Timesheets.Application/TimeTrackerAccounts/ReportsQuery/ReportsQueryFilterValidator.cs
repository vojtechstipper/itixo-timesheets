using FluentValidation;
using Itixo.Timesheets.Contracts.TimeEntries;

namespace Itixo.Timesheets.Application.TimeTrackerAccounts.ReportsQuery;

public class ReportsQueryFilterValidator : AbstractValidator<ReportsQueryFilter>
{
    public ReportsQueryFilterValidator()
    {
        RuleFor(x => x.ProjectIds).NotNull();
        RuleFor(x => x.ClientIds).NotNull();
    }
}
