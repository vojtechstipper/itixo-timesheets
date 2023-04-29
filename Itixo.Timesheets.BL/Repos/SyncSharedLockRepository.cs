using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.Configurations;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class SyncSharedLockRepository : ISyncSharedLockRepository
{
    private readonly IDbContext dbContext;

    public SyncSharedLockRepository(IDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task LockSynchornization(DateTimeOffset dateTimeNow)
    {
        var syncLock = SyncSharedLock.CreateLock(dateTimeNow);
        await dbContext.SyncSharedLocks.AddAsync(syncLock);
        await dbContext.SaveChangesAsync();
    }

    public async Task UnlockSynchornization(DateTimeOffset dateTimeNow)
    {
        SyncSharedLock synlock = await GetCurrentEntity();
        synlock.UnLock(dateTimeNow);
        dbContext.SyncSharedLocks.Update(synlock);
        await dbContext.SaveChangesAsync();
    }

    public async Task<SyncSharedLockContract> GetCurrent()
    {
        SyncSharedLock synlock = await GetCurrentEntity();
        return new SyncSharedLockContract {Start = synlock.Start, End = synlock.End, IsLocked = synlock.IsLocked()};
    }

    private async Task<SyncSharedLock> GetCurrentEntity()
    {
        return await dbContext.SyncSharedLocks.OrderByDescending(s => s.Start).FirstOrDefaultAsync() ?? SyncSharedLock.DEFAULT;
    }
}
