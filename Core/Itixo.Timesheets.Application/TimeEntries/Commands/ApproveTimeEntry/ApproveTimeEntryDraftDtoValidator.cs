using FluentValidation;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.ApproveTimeEntry;

public class ApproveTimeEntryDraftContractValidator : AbstractValidator<ApproveTimeEntryDraftContract>
{
    public ApproveTimeEntryDraftContractValidator()
    {
        RuleFor(x => x.TimeEntryDraftId).NotEmpty().GreaterThan(0);
    }
}
