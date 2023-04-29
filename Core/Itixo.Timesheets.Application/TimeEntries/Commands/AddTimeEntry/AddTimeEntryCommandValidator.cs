using FluentValidation;
using Itixo.Timesheets.Shared.Extensions;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.AddTimeEntry;

public class AddTimeEntryCommandValidator : AbstractValidator<AddTimeEntryCommand>
{
    public AddTimeEntryCommandValidator()
    {
        RuleFor(x => x.StartTime).NotEmpty();
        RuleFor(x => x.StartTime).NotDefaultDateTimeOffSet();
        RuleFor(x => x.StopTime).NotEmpty();
        RuleFor(x => x.StopTime).NotDefaultDateTimeOffSet();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.TimeTrackerAccountId).NotEmpty();
    }
}
