using FluentValidation;

namespace Itixo.Timesheets.SyncApi.Commands;

public class RunSynchronizationCommandValidator : AbstractValidator<RunSynchronizationCommand>
{
    public RunSynchronizationCommandValidator()
    {
        When(x => x.ManualRun, () =>
          {
              RuleFor(x => x.StartDate).NotEmpty();
              RuleFor(x => x.EndDate).NotEmpty();
              RuleFor(x => x.IdentityExternalId).NotEmpty();
          });
    }
}
