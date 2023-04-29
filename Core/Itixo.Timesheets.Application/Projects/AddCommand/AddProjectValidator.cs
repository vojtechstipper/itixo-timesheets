using FluentValidation;
using Itixo.Timesheets.Contracts.Projects;

namespace Itixo.Timesheets.Application.Projects.AddCommand;

public class ProjectContractValidator : AbstractValidator<ProjectContract>
{
    public ProjectContractValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ExternalId).NotEmpty().GreaterThan(0);
    }
}
