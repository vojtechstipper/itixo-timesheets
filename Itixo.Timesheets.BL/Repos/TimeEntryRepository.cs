using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Domain.TimeEntries.States;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;
using Itixo.Timesheets.Persistence;
using Itixo.Timesheets.Shared.Exceptions;
using Itixo.Timesheets.Shared.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Itixo.Timesheets.Infrastructure.Repos;

public class TimeEntryRepository : AppRepositoryBase<TimeEntry, int>, ITimeEntryRepository
{
    private readonly ICurrentIdentityProvider currentIdentityProvider;

    public TimeEntryRepository(IDbContext dbContext, ICurrentIdentityProvider currentIdentityProvider) : base(dbContext)
    {
        this.currentIdentityProvider = currentIdentityProvider;
    }

    public async Task UpdateAsync(TimeEntry draft)
    {
        dbContext.TimeEntries.Update(draft);
        await dbContext.SaveChangesAsync();
    }

    public async Task<int> AddAsync(TimeEntry timeEntry)
    {
        await dbContext.TimeEntries.AddAsync(timeEntry);
        await dbContext.SaveChangesAsync();
        return timeEntry.Id;
    }

    public async Task<int> BanTimeEntryDraftAsync(int id)
    {
        if (!await dbContext.TimeEntries.AnyAsync(x => x.Id == id))
        {
            throw new NotFoundException($"TimeEntryDraft with Id {id} was not found");
        }

        TimeEntry timeEntryDraft = await dbContext.TimeEntries
            .Include(x => x.TimeTrackerAccount)
            .Include(x => x.TimeEntryParams.Project)
            .FirstAsync(x => x.Id == id);

        IdentityInfo identityInfo = await dbContext.IdentityInfos.FirstAsync(f => f.ExternalId == currentIdentityProvider.ExternalId);

        var transitionParams = TransitionParams.Create(identityInfo, TimeEntryStateChangeReasons.StartTimeInFuture);
        timeEntryDraft.StateContext.CurrentState.ToBan(timeEntryDraft.StateContext, transitionParams);

        dbContext.TimeEntries.Update(timeEntryDraft);
        await dbContext.SaveChangesAsync();

        return id;
    }

    public Task<bool> AnyAsync(int id)
    {
        return dbContext.TimeEntries.AnyAsync(x => x.Id == id);
    }

    public async Task UpdateRangeAsync(IEnumerable<TimeEntry> timeEntriesToUpdate)
    {
        dbContext.TimeEntries.UpdateRange(timeEntriesToUpdate);
        await dbContext.SaveChangesAsync();
    }

    public async Task SynchronizeAsync(
        List<TimeEntry> insertTimeEntries,
        IEnumerable<TimeEntryVersion> insertTimeEntryVersions,
        List<TimeEntry> updateTimeEntries)
    {
        dbContext.TimeEntries.AddRange(insertTimeEntries);
        dbContext.TimeEntryVersions.AddRange(insertTimeEntryVersions);
        dbContext.TimeEntries.UpdateRange(updateTimeEntries);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteTimeEntriesAsync(IEnumerable<int> ids)
    {
        var entries = dbContext.TimeEntries.Where(entry => ids.Contains(entry.Id)).ToList();
        dbContext.TimeEntries.RemoveRange(entries);
        await dbContext.SaveChangesAsync();
    }
}
