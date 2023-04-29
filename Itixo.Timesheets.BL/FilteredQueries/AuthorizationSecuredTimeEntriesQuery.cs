using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Shared.Services;
using System.Linq;
using Itixo.Timesheets.Persistence;

namespace Itixo.Timesheets.Infrastructure.FilteredQueries;

public class AuthorizationSecuredTimeEntriesQuery : AuthorizationSecuredQueryBase<TimeEntry, int>, IPersistenceQuery<TimeEntry, int>

{
    public AuthorizationSecuredTimeEntriesQuery(ICurrentIdentityProvider currentIdentityProvider, IDbContext dbContext) : base(currentIdentityProvider, dbContext)
    {
    }

    public override IQueryable<TimeEntry> GetQueryable() => dbContext.Set<TimeEntry>()
        .Where(t => t.TimeTrackerAccount.Identities.Any(x => x.IdentityInfo.ExternalId == externalId) || isAdmin || isTimeSheetApp);
}
