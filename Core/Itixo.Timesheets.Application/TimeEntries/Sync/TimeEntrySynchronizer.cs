using AutoMapper;
using Itixo.Timesheets.Application.Services.FilteredQueries;
using Itixo.Timesheets.Application.Services.RepositoryAbstractions;
using Itixo.Timesheets.Contracts.Sync;
using Itixo.Timesheets.Contracts.TimeEntries.Approveds;
using Itixo.Timesheets.Domain;
using Itixo.Timesheets.Domain.Application;
using Itixo.Timesheets.Domain.TimeEntries;
using Itixo.Timesheets.Domain.TimeEntries.States;
using Itixo.Timesheets.Domain.TimeEntries.States.StateMachine.Utilities;
using Itixo.Timesheets.Domain.TimeTrackers;
using Itixo.Timesheets.Shared.Abstractions;
using Itixo.Timesheets.Shared.Extensions;
using Itixo.Timesheets.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Itixo.Timesheets.Application.TimeEntries.Sync;

public interface ITimeEntrySynchronizer : IService
{
    Task Process(List<TimeEntryContract> timeEntryContracts);
    Task Process(SyncWithLogParameter parameter);
}

public class TimeEntrySynchronizer : ITimeEntrySynchronizer
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly ISyncBatchLogRepository syncBatchLogRepository;
    private readonly ICurrentIdentityProvider currentIdentityProvider;
    private readonly IPersistenceQuery<IdentityInfo, int> identityInfosQuery;
    private readonly IPersistenceQuery<Project, int> projectsQuery;
    private readonly IPersistenceQuery<TimeTrackerAccount, int> timeTrackerAccountQuery;
    private readonly IPersistenceQuery<TimeEntry, int> timeEntryQuery;
    private readonly IConfiguration configuration;
    private readonly ITimeEntryRepository timeEntryRepository;
    private readonly IMapper mapper;

    private readonly List<TimeEntry> insertTimeEntries = new List<TimeEntry>();
    private readonly List<TimeEntry> updateTimeEntries = new List<TimeEntry>();

    private List<Project> projects = new List<Project>();
    private List<TimeTrackerAccount> accounts = new List<TimeTrackerAccount>();
    private IdentityInfo currentIdentityInfo;

    public TimeEntrySynchronizer(
        IDateTimeProvider dateTimeProvider,
        ISyncBatchLogRepository syncBatchLogRepository,
        ICurrentIdentityProvider currentIdentityProvider,
        IMapper mapper,
        IPersistenceQuery<IdentityInfo, int> identityInfosQuery,
        IPersistenceQuery<Project, int> projectsQuery,
        IPersistenceQuery<TimeTrackerAccount, int> timeTrackerAccountQuery,
        ITimeEntryRepository timeEntryRepository,
        IPersistenceQuery<TimeEntry, int> timeEntryQuery,
        IConfiguration configuration)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.syncBatchLogRepository = syncBatchLogRepository;
        this.currentIdentityProvider = currentIdentityProvider;
        this.mapper = mapper;
        this.identityInfosQuery = identityInfosQuery;
        this.projectsQuery = projectsQuery;
        this.timeTrackerAccountQuery = timeTrackerAccountQuery;
        this.timeEntryRepository = timeEntryRepository;
        this.timeEntryQuery = timeEntryQuery;
        this.configuration = configuration;
    }

    public async Task Process(SyncWithLogParameter parameter)
    {
        var syncBatchLogRecord = SyncBatchLogRecord.CreateStartedLogRecord(dateTimeProvider.Now, parameter.SyncLogRecordId);
        SynchronizationResult result = await Synchronize(parameter.TimeEntryContracts);
        LogStoppingOfSyncBatch(syncBatchLogRecord, result);
        await syncBatchLogRepository.AddAsync(syncBatchLogRecord);
    }

    private void LogStoppingOfSyncBatch(SyncBatchLogRecord syncBatchLogRecord, SynchronizationResult result) =>
        syncBatchLogRecord.StopStartedLog(
            dateTimeProvider.Now,
            result.InsertedDraftedRecordsIds,
            result.InsertedApprovedRecordsIds,
            result.UpdatedDraftedRecordsIds,
            result.UpdatedApprovedRecordsIds);

    public async Task Process(List<TimeEntryContract> timeEntryContracts)
    {
        await Synchronize(timeEntryContracts);
    }

    private async Task<SynchronizationResult> Synchronize(List<TimeEntryContract> timeEntryContracts)
    {
        var timeEntryExternalIds = timeEntryContracts.Select(s => s.ExternalTimeEntryId ?? default).Distinct().ToList();
        var timeTrackerExternalIds = timeEntryContracts.Select(s => s.TimeTrackerExternalId).Distinct().ToList();
        var projectExternalIds = timeEntryContracts.Select(s => s.ExternalProjectId).Distinct().ToList();
        currentIdentityInfo = await identityInfosQuery.GetQueryable().FirstOrDefaultAsync(f => f.ExternalId == currentIdentityProvider.ExternalId);

        accounts = await timeTrackerAccountQuery.GetQueryable().Where(u => timeTrackerExternalIds.Contains(u.ExternalId)).ToListAsync();

        List<TimeEntry> timeEntries = await GetTimeEntries(timeEntryExternalIds, accounts);

        projects = await projectsQuery.GetQueryable()
            .Where(project => projectExternalIds.Contains(project.ExternalId))
            .ToListAsync();

        foreach (TimeTrackerAccount account in accounts)
        {
            foreach (TimeEntryContract timeEntryContract in timeEntryContracts)
            {
                TimeEntry timeEntry = timeEntries.FirstOrDefault(f => f.TimeEntryParams.ExternalId == timeEntryContract.ExternalTimeEntryId);
                Project project = projects.FirstOrDefault(f => f.ExternalId == timeEntryContract.ExternalProjectId);

                if (timeEntry != null)
                {
                    if (timeEntryContract.EqualsTimeEntryParams(timeEntry.TimeEntryParams))
                    {
                        continue;
                    }

                    timeEntry.Update(CreateTimeEntryParams(timeEntryContract, project), account, dateTimeProvider.Now.ToUniversalTime());
                    ConsiderChangingStateToDraft(timeEntryContract, timeEntry);

                    updateTimeEntries.Add(timeEntry);
                }
                else
                {
                    TimeEntryParams timeEntryParams = CreateTimeEntryParams(timeEntryContract, project);
                    var transitionParams = TransitionParams.Create(currentIdentityInfo, TimeEntryStateChangeReasons.Ok);
                    timeEntry = TimeEntry.CreateApproved(timeEntryParams, account, dateTimeProvider.Now, transitionParams);

                    if (WasLatelyModified(timeEntryContract))
                    {
                        transitionParams = TransitionParams.Create(currentIdentityInfo, TimeEntryStateChangeReasons.ModifiedLately);
                        timeEntry = TimeEntry.CreateDraft(timeEntryParams, account, dateTimeProvider.Now, transitionParams);
                    }

                    if (IsStartTimeInFuture(timeEntryContract))
                    {
                        transitionParams = TransitionParams.Create(currentIdentityInfo, TimeEntryStateChangeReasons.StartTimeInFuture);
                        timeEntry = TimeEntry.CreateDraft(timeEntryParams, account, dateTimeProvider.Now, transitionParams);
                    }

                    if (IsFictionalProject(timeEntryContract, Convert.ToInt64(configuration["FictionalProject:ExternalId"])))
                    {
                        transitionParams = TransitionParams.Create(currentIdentityInfo, TimeEntryStateChangeReasons.FictionalProject);
                        timeEntry = TimeEntry.CreateDraft(timeEntryParams, account, dateTimeProvider.Now, transitionParams);
                    }
                    insertTimeEntries.Add(timeEntry);
                }
            }
        }

        await PreDeleteMissingInBatchTimeEntries(timeEntryContracts);

        IEnumerable<TimeEntryVersion> insertTimeEntryVersions = updateTimeEntries.SelectMany(s => s.TimeEntryVersions).Where(w => w.Id == 0);
        await timeEntryRepository.SynchronizeAsync(insertTimeEntries, insertTimeEntryVersions, updateTimeEntries);
        return CreateResult();
    }

    private static bool IsStartTimeInFuture(TimeEntryContract timeEntryContract) => timeEntryContract.StartTime > timeEntryContract.LastModifiedDate;

    private static bool WasLatelyModified(TimeEntryContract timeEntryContract) => timeEntryContract.StartTime < timeEntryContract.LastModifiedDate.Date.AddDays(1).AddBusinessDays(-3);

    private static bool IsFictionalProject(TimeEntryContract timeEntryContract, long fictionalProjectExternalId) => timeEntryContract.ExternalProjectId == fictionalProjectExternalId;

    private void ConsiderChangingStateToDraft(TimeEntryContract timeEntryContract, TimeEntry timeEntry)
    {
        if (WasLatelyModified(timeEntryContract))
        {
            var transitionParams = TransitionParams.Create(currentIdentityInfo, TimeEntryStateChangeReasons.ModifiedLately);
            timeEntry.StateContext.CurrentState.ToDraft(timeEntry.StateContext, transitionParams);
        }

        if (IsStartTimeInFuture(timeEntryContract))
        {
            var transitionParams = TransitionParams.Create(currentIdentityInfo, TimeEntryStateChangeReasons.StartTimeInFuture);
            timeEntry.StateContext.CurrentState.ToDraft(timeEntry.StateContext, transitionParams);
        }

        if (IsFictionalProject(timeEntryContract, Convert.ToInt64(configuration["FictionalProject:ExternalId"])))
        {
            var transitionParams = TransitionParams.Create(currentIdentityInfo, TimeEntryStateChangeReasons.FictionalProject);
            timeEntry.StateContext.CurrentState.ToDraft(timeEntry.StateContext, transitionParams);
        }
    }

    private async Task PreDeleteMissingInBatchTimeEntries(List<TimeEntryContract> timeEntryContracts)
    {
        DateTimeOffset startTime = GetOldestStartTime(timeEntryContracts);
        DateTimeOffset stopTime = GetYoungestStopTime(timeEntryContracts);
        List<TimeEntry> timeEntries = await GetTimeEntries(startTime, stopTime, accounts);

        IEnumerable<TimeEntry> missingInBatchTimeEntries = timeEntries
            .Where(timeEntry => timeEntryContracts.All(contract => contract.ExternalTimeEntryId != timeEntry.TimeEntryParams.ExternalId));

        foreach (TimeEntry missingInBatchTimeEntry in missingInBatchTimeEntries)
        {
            var transitionParams = TransitionParams.Create(currentIdentityInfo, TimeEntryStateChangeReasons.MissingInBatch);
            missingInBatchTimeEntry.StateContext.CurrentState.ToPreDeleted(missingInBatchTimeEntry.StateContext, transitionParams);
            updateTimeEntries.Add(missingInBatchTimeEntry);
        }
    }

    private static DateTimeOffset GetYoungestStopTime(List<TimeEntryContract> timeEntryContracts)
        => timeEntryContracts.Select(s => s.StopTime).OrderByDescending(value => value).First();

    private static DateTimeOffset GetOldestStartTime(List<TimeEntryContract> timeEntryContracts)
        => timeEntryContracts.Select(s => s.StartTime).OrderBy(value => value).First();

    private async Task<List<TimeEntry>> GetTimeEntries(DateTimeOffset startTime, DateTimeOffset stopTime, List<TimeTrackerAccount> accounts) =>
        await timeEntryQuery.GetQueryable()
            .Include(x => x.TimeTrackerAccount)
            .Include(x => x.TimeEntryVersions)
            .Where(
                te => te.TimeEntryParams.StartTime.CompareTo(startTime) >= 0
                      && te.TimeEntryParams.StopTime.CompareTo(stopTime) <= 0
                      && accounts.Select(s => s.Id).Contains(te.TimeTrackerAccountId))
            .ToListAsync();

    private async Task<List<TimeEntry>> GetTimeEntries(IEnumerable<long> externalIds, List<TimeTrackerAccount> accounts) =>
        await timeEntryQuery.GetQueryable()
            .Include(x => x.TimeTrackerAccount)
            .Include(x => x.TimeEntryVersions)
            .Where(
                te => externalIds.Contains(te.TimeEntryParams.ExternalId ?? default)
                      && accounts.Select(s => s.Id).Contains(te.TimeTrackerAccountId))
            .ToListAsync();

    private SynchronizationResult CreateResult()
    {
        return new SynchronizationResult
        {
            InsertedApprovedRecordsIds = insertTimeEntries.Where(w => w.StateContext.IsApproved).Select(s => s.Id).ToList(),
            InsertedDraftedRecordsIds = insertTimeEntries.Where(w => w.StateContext.IsDraft).Select(s => s.Id).ToList(),
            UpdatedApprovedRecordsIds = updateTimeEntries.Where(w => w.StateContext.IsApproved).Select(s => s.Id).ToList(),
            UpdatedDraftedRecordsIds = updateTimeEntries.Where(w => w.StateContext.IsDraft).Select(s => s.Id).ToList(),
        };
    }

    private TimeEntryParams CreateTimeEntryParams(TimeEntryContract timeEntryContract, Project project)
    {
        TimeEntryParams timeEntryParams = mapper.Map<TimeEntryParams>(timeEntryContract);
        timeEntryParams.Project = project;
        return timeEntryParams;
    }
}

class SynchronizationResult
{
    public IList<int> InsertedDraftedRecordsIds { get; set; } = new List<int>();
    public IList<int> UpdatedApprovedRecordsIds { get; set; } = new List<int>();
    public IList<int> InsertedApprovedRecordsIds { get; set; } = new List<int>();
    public IList<int> UpdatedDraftedRecordsIds { get; set; } = new List<int>();
}
