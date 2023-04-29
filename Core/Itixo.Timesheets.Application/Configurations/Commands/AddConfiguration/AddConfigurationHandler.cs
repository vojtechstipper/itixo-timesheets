using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain.Application;
using MediatR;

namespace Itixo.Timesheets.Application.Configurations.Commands.AddConfiguration;

public class AddConfigurationHandler : IRequestHandler<AddConfigurationCommand, AddConfigurationResponse>
{
    private readonly IConfigurationRepository configurationRepository;
    private readonly IMapper mapper;

    public AddConfigurationHandler(IConfigurationRepository configurationRepository, IMapper mapper)
    {
        this.configurationRepository = configurationRepository;
        this.mapper = mapper;
    }

    public async Task<AddConfigurationResponse> Handle(AddConfigurationCommand request, CancellationToken cancellationToken)
    {
        Configuration configuration = mapper.Map<Configuration>(request);
        int id = await configurationRepository.InsertAsync(configuration, cancellationToken);
        return new AddConfigurationResponse { ConfigurationId = id };
    }
}
