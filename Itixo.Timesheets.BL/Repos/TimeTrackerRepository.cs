using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Persistence;
using Itixo.Timesheets.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class TimeTrackerRepository : AppRepositoryBase<TimeTracker, int>, ITimeTrackerRepository
{
    public TimeTrackerRepository(IDbContext dbContext) : base(dbContext) { }

    public Task<TimeTracker> GetTimeTrackerByTypeAsync(TimeTrackerType type)
    {
        return GetDbSet().FirstOrDefaultAsync(f => f.Type == type);
    }
}
