using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Services;
using System.Linq;
using Itixo.Timesheets.Persistence;

namespace Itixo.Timesheets.Infrastructure.FilteredQueries;

public abstract class AuthorizationSecuredQueryBase<T, TKey>
where T : IEntity<TKey>
{
    protected readonly IDbContext dbContext;

    protected bool isAdmin;
    protected bool isTimeSheetApp;
    protected string externalId;

    protected AuthorizationSecuredQueryBase(ICurrentIdentityProvider currentIdentityProvider, IDbContext dbContext)
    {
        this.dbContext = dbContext;
        isAdmin = currentIdentityProvider.IsAdmin();
        isTimeSheetApp = currentIdentityProvider.IsTimeSheetApp();
        externalId = currentIdentityProvider.ExternalId;
    }

    public abstract IQueryable<T> GetQueryable();
}
