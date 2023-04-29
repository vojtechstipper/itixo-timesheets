using FluentValidation;
using Itixo.Timesheets.Contracts.Workspaces;

namespace Itixo.Timesheets.Application.Workspaces.AddCommand;

public class AddWorkspaceValidator : AbstractValidator<AddWorkspaceContract>
{
    public AddWorkspaceValidator()
    {
        RuleFor(x => x.WorkspaceName).NotEmpty();
        RuleFor(x => x.ExternalId).GreaterThan(0);
    }
}
