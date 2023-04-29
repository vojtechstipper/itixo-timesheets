using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Shared.Services;
using MediatR;

namespace Itixo.Timesheets.Application.IdentityInfos.Commands.UpdateCurrent;

public class UpdateCurrentIdentityInfoHandler : IRequestHandler<UpdateCurrentIdentityInfoCommand, Unit>
{
    private readonly ICurrentIdentityProvider currentIdentityProvider;
    private readonly IIdentityInfoRepository identityInfoRepository;

    public UpdateCurrentIdentityInfoHandler(ICurrentIdentityProvider currentIdentityProvider, IIdentityInfoRepository identityInfoRepository)
    {
        this.currentIdentityProvider = currentIdentityProvider;
        this.identityInfoRepository = identityInfoRepository;
    }

    public async Task<Unit> Handle(UpdateCurrentIdentityInfoCommand request, CancellationToken cancellationToken)
    {
        IdentityInfo identityInfo = await identityInfoRepository.GetIdentityInfoAsync(currentIdentityProvider.ExternalId);
        identityInfo.Email = request.Email;
        await identityInfoRepository.UpdateAsync(identityInfo, cancellationToken);
        return Unit.Value;
    }
}