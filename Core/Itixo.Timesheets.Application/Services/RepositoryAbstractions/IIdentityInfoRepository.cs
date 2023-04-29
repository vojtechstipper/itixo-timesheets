using System.Threading.Tasks;
using Itixo.Timesheets.Domain.Application;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface IIdentityInfoRepository : IEntityRepository<IdentityInfo, int>
{
    Task<IdentityInfo> GetIdentityInfoAsync(string externalId);
}
