using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Shared.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Itixo.Timesheets.Application.Services.Shared;

public interface ITogglSyncIdentityProvider : IService
{
    Task<IdentityInfo> GetOrCreateIdentityAsync();
}

public class TogglSyncIdentityProvider : ITogglSyncIdentityProvider
{
    private readonly IConfiguration configuration;
    private readonly IIdentityInfoRepository identityInfoRepository;

    public TogglSyncIdentityProvider(IConfiguration configuration, IIdentityInfoRepository identityInfoRepository)
    {
        this.configuration = configuration;
        this.identityInfoRepository = identityInfoRepository;
    }

    public async Task<IdentityInfo> GetOrCreateIdentityAsync()
    {
        IdentityInfo identityInfo = await identityInfoRepository.GetIdentityInfoAsync(configuration["TogglSync:ExternalId"]);

        if (identityInfo != null)
        {
            return identityInfo;
        }

        identityInfo = IdentityInfo.From(configuration["TogglSync:ExternalId"], configuration["TogglSync:Identifier"]);
        await identityInfoRepository.InsertAsync(identityInfo);
        return identityInfo;
    }
}
