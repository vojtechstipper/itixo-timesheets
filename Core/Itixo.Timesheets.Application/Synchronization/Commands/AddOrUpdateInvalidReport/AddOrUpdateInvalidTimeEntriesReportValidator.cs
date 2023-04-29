using FluentValidation;

namespace Itixo.Timesheets.Application.Synchronization.Commands.AddOrUpdateInvalidReport;

public class AddOrUpdateInvalidTimeEntriesReportValidator : AbstractValidator<AddOrUpdateInvalidTimeEntriesReportCommand>
{
    public AddOrUpdateInvalidTimeEntriesReportValidator()
    {
        RuleFor(f => f.ReceiverEmailAddress).NotEmpty();
    }
}
