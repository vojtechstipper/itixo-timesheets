using FluentValidation;
using Itixo.Timesheets.Contracts.TimeEntries.Bans;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Application.TimeEntries.Commands.BanTimeEntry;

public class BanTimeEntryDraftContractValidator : AbstractValidator<BanTimeEntryDraftContract>
{
    public BanTimeEntryDraftContractValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .GreaterThan(0)
            .WithMessage(nameof(Validations.BanTimeEntryDraftValidator_ValidationMessage_Id_Greater_Than_Zero));
    }
}
