using System.Threading.Tasks;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Enums;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface ITimeTrackerRepository : IEntityRepository<TimeTracker, int>
{
    Task<TimeTracker> GetTimeTrackerByTypeAsync(TimeTrackerType type);
}
