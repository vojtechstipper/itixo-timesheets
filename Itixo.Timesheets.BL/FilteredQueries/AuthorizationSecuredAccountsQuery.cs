using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Persistence;
using Itixo.Timesheets.Shared.Services;
using System.Linq;

namespace Itixo.Timesheets.Infrastructure.FilteredQueries;

public class AuthorizationSecuredAccountsQuery : AuthorizationSecuredQueryBase<TimeTrackerAccount, int>, IPersistenceQuery<TimeTrackerAccount, int>
{
    public AuthorizationSecuredAccountsQuery(ICurrentIdentityProvider currentIdentityProvider, IDbContext dbContext) : base(currentIdentityProvider, dbContext)
    {
    }

    public override IQueryable<TimeTrackerAccount> GetQueryable()
    {
        return dbContext.TimeTrackerAccounts.Where(acc => acc.Identities.Any(x => x.IdentityInfo.ExternalId == externalId) || isAdmin);
    }
}
