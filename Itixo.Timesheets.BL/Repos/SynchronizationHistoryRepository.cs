using System;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class SynchronizationHistoryRepository : ISynchronizationHistoryRepository
{
    private readonly AppDbContext dbContext;

    public SynchronizationHistoryRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(SyncLogRecord record)
    {
        record.IdentityInfo = await dbContext.IdentityInfos.FirstAsync(ii => ii.ExternalId == record.IdentityInfo.ExternalId);
        await dbContext.SyncLogRecords.AddAsync(record);
        await dbContext.SaveChangesAsync();
    }

    public Task<SyncLogRecord> GetAsync(Guid id)
    {
        return dbContext.SyncLogRecords.Include(x => x.IdentityInfo).FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task UpdateAsync(SyncLogRecord syncLogRecord)
    {
        syncLogRecord.IdentityInfo = await dbContext.IdentityInfos.FirstAsync(ii => ii.ExternalId == syncLogRecord.IdentityInfo.ExternalId);
        dbContext.SyncLogRecords.Update(syncLogRecord);
        await dbContext.SaveChangesAsync();
    }
}
