using FluentValidation;
using Itixo.Timesheets.Contracts.SyncHistory;
using Itixo.Timesheets.Shared.Extensions;
using Itixo.Timesheets.Shared.Resources;

namespace Itixo.Timesheets.Application.SyncHistory.StartSyncLogCommand;

public class LogSyncCommandValidator : AbstractValidator<LogSyncCommand>
{
    public LogSyncCommandValidator()
    {
        RuleFor(x => x.IdentityExternalId).NotEmpty();
        RuleFor(x => x.StartedDate).NotEmpty();
        RuleFor(x => x.StartedDate).NotDefaultDateTimeOffSet();
        RuleFor(x => x.SyncedFrom).NotEmpty();
        RuleFor(x => x.SyncedFrom).NotDefaultDateTimeOffSet();
        RuleFor(x => x.SyncedTo).NotEmpty();
        RuleFor(x => x.SyncedTo).NotDefaultDateTimeOffSet();

        When(
            x => x.Kind == LogSyncCommand.SyncResultKind.Success,
            () =>
            {
                RuleFor(x => x.LogSyncRecordId).NotEmpty();
                RuleFor(x => x.StoppedDate).NotEmpty();
                RuleFor(x => x.StoppedDate).Must(stoppedDate => stoppedDate != default)
                    .WithMessage(Validations.LogSyncCommandValidator_LogSyncCommandValidator_Date_cannot_be_default);
            });
        When(
            x => x.Kind == LogSyncCommand.SyncResultKind.Error,
            () =>
            {
                RuleFor(x => x.LogSyncRecordId).NotEmpty();
                RuleFor(x => x.StoppedDate).NotEmpty();
                RuleFor(x => x.StoppedDate).Must(stoppedDate => stoppedDate != default)
                    .WithMessage(Validations.LogSyncCommandValidator_LogSyncCommandValidator_Date_cannot_be_default);
                RuleFor(x => x.ErrorMessage).NotEmpty();
                RuleFor(x => x.ErrorStackTrace).NotEmpty();
            });
    }
}
