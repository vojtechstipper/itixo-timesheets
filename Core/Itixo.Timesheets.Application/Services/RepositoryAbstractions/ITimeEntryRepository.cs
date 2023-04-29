using Itixo.Timesheets.Domain.TimeEntries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.Services.RepositoryAbstractions;

public interface ITimeEntryRepository : IEntityRepository<TimeEntry, int>
{
    Task UpdateAsync(TimeEntry draft);
    Task<int> AddAsync(TimeEntry timeEntry);
    Task<int> BanTimeEntryDraftAsync(int id);
    Task DeleteTimeEntriesAsync(IEnumerable<int> ids);
    Task<bool> AnyAsync(int id);
    Task UpdateRangeAsync(IEnumerable<TimeEntry> timeEntriesToUpdate);
    Task SynchronizeAsync(List<TimeEntry> insertTimeEntries, IEnumerable<TimeEntryVersion> insertTimeEntryVersions, List<TimeEntry> updateTimeEntries);
}
