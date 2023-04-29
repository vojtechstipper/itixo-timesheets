using System.Threading;
using System.Threading.Tasks;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain.Synchronization;
using Itixo.Timesheets.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class InvalidTimeEntriesReportRepository : AppRepositoryBase<InvalidTimeEntriesReport, int>, IInvalidTimeEntriesReportRepository
{
    public InvalidTimeEntriesReportRepository(IDbContext dbContext) : base(dbContext)
    {
    }

    public Task<InvalidTimeEntriesReport> GetByEmailAsync(string email, CancellationToken token = default)
    {
        return GetDbSet().FirstOrDefaultAsync(f => f.ReceiverEmailAddress == email, token);
    }
}
