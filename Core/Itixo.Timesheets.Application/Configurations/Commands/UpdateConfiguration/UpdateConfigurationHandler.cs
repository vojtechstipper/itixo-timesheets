using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain.Application;
using MediatR;

namespace Itixo.Timesheets.Application.Configurations.Commands.UpdateConfiguration;

public class UpdateConfigurationHandler : IRequestHandler<UpdateConfigurationCommand, Unit>
{
    private readonly IConfigurationRepository configurationRepository;

    public UpdateConfigurationHandler(IConfigurationRepository configurationRepository)
    {
        this.configurationRepository = configurationRepository;
    }

    public async Task<Unit> Handle(UpdateConfigurationCommand request, CancellationToken cancellationToken)
    {
        Configuration configuration = await configurationRepository.GetByKeyAsync(request.Key, cancellationToken);
        configuration.Value = request.Value;
        await configurationRepository.UpdateAsync(configuration, cancellationToken);
        return Unit.Value;
    }
}
