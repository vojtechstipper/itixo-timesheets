using System.Linq;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Persistence;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class SyncBatchLogRepository : ISyncBatchLogRepository
{
    private readonly AppDbContext dbContext;

    public SyncBatchLogRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddAsync(SyncBatchLogRecord record)
    {
        record.SyncLogRecord = dbContext.SyncLogRecords.First(f => f.Id == record.SyncLogRecord.Id);
        await dbContext.SyncBatchLogRecords.AddAsync(record);
        await dbContext.SaveChangesAsync();
    }
}
