using FluentValidation;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.BanTimeEntry;

public class BanTimeEntryCommandValidator : AbstractValidator<BanTimeEntryCommand>
{
    public BanTimeEntryCommandValidator()
    {
        RuleForEach(x => x.TimeEntries).SetValidator(new BanTimeEntryDraftContractValidator());
    }
}
