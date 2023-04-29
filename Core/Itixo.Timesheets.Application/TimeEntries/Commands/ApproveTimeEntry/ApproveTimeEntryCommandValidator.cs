using FluentValidation;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.ApproveTimeEntry;

public class ApproveTimeEntryCommandValidator : AbstractValidator<ApproveTimeEntryCommand>
{
    public ApproveTimeEntryCommandValidator()
    {
        RuleForEach(x => x.TimeEntries).SetValidator(new ApproveTimeEntryDraftContractValidator());
    }
}
