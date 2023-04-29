using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class IdentityInfoRepository : AppRepositoryBase<IdentityInfo, int>, IIdentityInfoRepository
{
    public IdentityInfoRepository(IDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IdentityInfo> GetIdentityInfoAsync(string externalId)
    {
        return await dbContext.IdentityInfos.FirstOrDefaultAsync(f => f.ExternalId == externalId);
    }
}
