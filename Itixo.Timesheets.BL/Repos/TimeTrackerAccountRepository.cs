using AutoMapper;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.TimeTrackerAccounts;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Persistence;
using Itixo.Timesheets.Shared.Enums;
using Itixo.Timesheets.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class TimeTrackerAccountRepository : AppRepositoryBase<TimeTrackerAccount, int>, ITimeTrackerAccountRepository
{
    private readonly IMapper mapper;
    private readonly IPersistenceQuery<TimeTrackerAccount, int> timeTrackerAccountQuery;

    public TimeTrackerAccountRepository(IDbContext dbContext, IMapper mapper, IPersistenceQuery<TimeTrackerAccount, int> timeTrackerAccountQuery) : base(dbContext)
    {
        this.mapper = mapper;
        this.timeTrackerAccountQuery = timeTrackerAccountQuery;
    }

    public async Task<TimeTrackerAccount> GetUserAsync(string apiToken)
    {
        return await timeTrackerAccountQuery.GetQueryable().SingleOrDefaultAsync(u => u.ExternalId == apiToken);
    }

    public async Task<TimeTrackerAccount> GetAsync(int id)
    {
        return await timeTrackerAccountQuery.GetQueryable().Include(x => x.Identities).ThenInclude(x => x.IdentityInfo).FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<AccountDetailContract> GetUsersTogglAccountDetailsAsync(string username)
    {
        IQueryable<TimeTrackerAccount> query = timeTrackerAccountQuery.GetQueryable().Where(w => w.Username == username);

        AccountDetailContract result = await mapper.ProjectTo<AccountDetailContract>(query).FirstOrDefaultAsync();

        if (result == null)
        {
            throw new NotFoundException($"TimeTrackerAccount with username {username} was not found in database");
        }

        return result;
    }

    public async Task<int> InsertAsync(TimeTrackerAccount account)
    {
        AdaptTimeTracker(account);

        List<IdentityInfo> alreadyPersistedIdentityInfo = await dbContext.IdentityInfos
            .Where(w => account.Identities.Select(s => s.IdentityInfo.ExternalId).Contains(w.ExternalId))
            .ToListAsync();

        foreach (IdentityTimeTrackerAccount identityTimeTrackerAccount in account.Identities)
        {
            IdentityInfo identityInfo = alreadyPersistedIdentityInfo
                .FirstOrDefault(f => f.ExternalId == identityTimeTrackerAccount.IdentityInfo.ExternalId);

            if (identityInfo != null)
            {
                identityInfo.Email = identityTimeTrackerAccount.IdentityInfo.Email;
                identityInfo.Identifier = identityTimeTrackerAccount.IdentityInfo.Identifier;
                identityTimeTrackerAccount.IdentityInfo = identityInfo;
                dbContext.IdentityInfos.Update(identityInfo);
            }
        }

        await dbContext.TimeTrackerAccounts.AddAsync(account);
        await dbContext.SaveChangesAsync();
        return account.Id;
    }

    public async Task UpdateAsync(TimeTrackerAccount account)
    {
        foreach (IdentityTimeTrackerAccount identityTimeTrackerAccount in account.Identities)
        {
            IdentityInfo identityInfo = identityTimeTrackerAccount.IdentityInfo;

            if (identityInfo.Id == 0 && dbContext.IdentityInfos.Any(x => x.ExternalId == identityInfo.ExternalId))
            {
                identityTimeTrackerAccount.IdentityInfo = dbContext.IdentityInfos.First(x => x.ExternalId == identityInfo.ExternalId);
                dbContext.Entry(identityTimeTrackerAccount).State = EntityState.Unchanged;
            }
        }

        AdaptTimeTracker(account);
        dbContext.TimeTrackerAccounts.Update(account);
        await dbContext.SaveChangesAsync();
    }


    public async Task DeleteAsync(int? id)
    {
        dbContext.TimeTrackerAccounts.Remove(dbContext.TimeTrackerAccounts.First(f => f.Id == id));
        await dbContext.SaveChangesAsync();
    }

    public Task<bool> AnyAsync(int id) => dbContext.TimeTrackerAccounts.AnyAsync(x => x.Id == id);

    public Task<TimeTrackerAccount> UnauthorizedGetByParamsAsync(string username, string externalId)
    {
        return dbContext.TimeTrackerAccounts.FirstOrDefaultAsync(f => f.Username == username && f.ExternalId == externalId);
    }

    public async Task<TimeTrackerAccount> UnauthorizedGetAsync(int id)
    {
        return await dbContext.TimeTrackerAccounts.Include(x => x.Identities).ThenInclude(x => x.IdentityInfo).FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<IEnumerable<T>> ListAsync<T>()
    {
        return await mapper.ProjectTo<T>(timeTrackerAccountQuery.GetQueryable()).ToListAsync();
    }

    public async Task<IEnumerable<T>> OnlyApplicationAccountsAsync<T>()
    {
        return await mapper.ProjectTo<T>(timeTrackerAccountQuery
            .GetQueryable()
            .Where(w => w.TimeTracker.Type == TimeTrackerType.ThisApplication ))
            .ToListAsync();
    }

    public Task<List<TimeTrackerAccount>> RawListAsync() => timeTrackerAccountQuery.GetQueryable().ToListAsync();

    private void AdaptTimeTracker(TimeTrackerAccount account)
    {
        if (account.TimeTracker != null && account.TimeTracker.Id > 0 && (account.TimeTracker.Name ?? string.Empty) == string.Empty)
        {
            account.TimeTracker = dbContext.Set<TimeTracker>().First(f => f.Id == account.TimeTracker.Id);
        }
    }
}
